using COPI_API.Models.DPEEntities;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COPI_API.Services;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using static COPI_API.Models.DPEEntities.Documento;
using static COPI_API.Models.DTO.DPEdto;

namespace COPI_API.Controllers.ControllerEmentario
{

    [ApiController]
    [Route("api/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly PdfService _pdfService;
        private readonly DocumentoParserService _parserService;

        public DocumentosController(
            IWebHostEnvironment env,
            AppDbContext context,
            PdfService pdfService,
            DocumentoParserService parserService)
        {
            _env = env;
            _context = context;
            _pdfService = pdfService;
            _parserService = parserService;
        }

        [Authorize(Roles = "Admin,Gestor")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentoDto dto)
        {
            if (dto.Arquivo == null || dto.Arquivo.Length == 0)
                return BadRequest("Arquivo obrigatório");

            if (!Enum.IsDefined(typeof(Afastamento.TipoAfastamento), dto.TipoAfastamento))
                return BadRequest("Tipo de afastamento inválido");

            if (!_context.Divisoes.Any(d => d.Id == dto.DivisaoId))
                return BadRequest("Divisão inválida");

            var uploadDir = Path.Combine(_env.ContentRootPath, "Uploads");

            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var nomeArmazenado =
                $"{Guid.NewGuid()}{Path.GetExtension(dto.Arquivo.FileName)}";

            var filePath = Path.Combine(uploadDir, nomeArmazenado);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Arquivo.CopyToAsync(stream);
            }

            var textoExtraido = _pdfService.ExtrairTexto(filePath);
            var ementa = _parserService.ExtrairEmenta(textoExtraido);
            var numeroSei = _parserService.ExtrairNumeroSei(textoExtraido);

            string? numeroProcesso = null;

            if (dto.Tipo == TipoDocumento.Manifestacao && string.IsNullOrEmpty(ementa))
            {
                // Se entrar aqui, o problema é o REGEX ou o TEXTO do PDF que veio sujo
                return BadRequest(new
                {
                    erro = "Não foi possível extrair a ementa do texto.",
                    textoBruto = textoExtraido.Substring(0, Math.Min(textoExtraido.Length, 500))
                });
            }

            if (dto.Tipo == TipoDocumento.Manifestacao)
            {
                ementa = _parserService.ExtrairEmenta(textoExtraido);
            }
            else if (dto.Tipo == TipoDocumento.DeclaracaoMotivacao)
            {
                numeroProcesso = _parserService.ExtrairNumeroProcesso(textoExtraido);
            }

            var afastamento = new Afastamento
            {
                Tipo = dto.TipoAfastamento,
                CriadoEm = DateTime.UtcNow,
                Status = Afastamento.StatusAfastamento.EmAnalise
            };

            _context.Afastamentos.Add(afastamento);
            await _context.SaveChangesAsync();

            var documento = new Documento
            {
                Id = Guid.NewGuid(),
                AfastamentoId = afastamento.Id,
                DivisaoId = dto.DivisaoId,
                NomeOriginal = dto.Arquivo.FileName,
                NomeArmazenado = nomeArmazenado,
                CaminhoArquivo = filePath,
                ContentType = dto.Arquivo.ContentType ?? "application/pdf",
                Tamanho = dto.Arquivo.Length,
                CriadoEm = DateTime.UtcNow,
                CriadoPor = User.Identity?.Name ?? "Sistema",
                Tipo = dto.Tipo,
                TextoExtraido = textoExtraido
            };

            if (ementa != null)
            {
                var ementario = new Ementario
                {
                    AfastamentoId = afastamento.Id,
                    EmentaResumo = ementa,
                    NumeroSei = numeroSei,
                    CriadoEm = DateTime.UtcNow
                };

                _context.Ementarios.Add(ementario);
            }

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Upload realizado",
                ementaExtraida = ementa != null,
                processoExtraido = numeroProcesso != null
            });
        }

        [Authorize]
        [HttpGet("{id}/download")]
        public IActionResult Download(Guid id)
        {
            var doc = _context.Documentos
                .AsNoTracking()
                .FirstOrDefault(d => d.Id == id);

            if (doc == null)
                return NotFound();

            if (!System.IO.File.Exists(doc.CaminhoArquivo))
                return NotFound("Arquivo físico não encontrado");

            return PhysicalFile(
                doc.CaminhoArquivo,
                doc.ContentType,
                doc.NomeOriginal
            );
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentoResponseDto>>> GetAll()
        {
            var documentos = await _context.Documentos
                .OrderByDescending(d => d.CriadoEm)
                .Select(d => new DocumentoResponseDto
                {
                    Id = d.Id,
                    AfastamentoId = d.AfastamentoId,
                    NomeOriginal = d.NomeOriginal,
                    Tipo = d.Tipo,
                    Tamanho = d.Tamanho,
                    CriadoEm = d.CriadoEm,
                    CriadoPor = d.CriadoPor
                })
                .ToListAsync();

            return Ok(documentos);
        }


        [Authorize(Roles = "Admin,Gestor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var doc = await _context.Documentos
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doc == null)
                return NotFound();

            if (System.IO.File.Exists(doc.CaminhoArquivo))
                System.IO.File.Delete(doc.CaminhoArquivo);

            _context.Documentos.Remove(doc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpGet("filtrar")]
        public async Task<IActionResult> GetDocumentos(
             [FromQuery] DateTime? dataInicio,
             [FromQuery] DateTime? dataFim
         )
        {
            var query = _context.Documentos.AsQueryable();

            if (dataInicio.HasValue)
            {
                var inicio = dataInicio.Value.Date;
                query = query.Where(d => d.CriadoEm >= inicio);
            }

            if (dataFim.HasValue)
            {
                var fimDoDia = dataFim.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(d => d.CriadoEm <= fimDoDia);
            }

            var documentos = await query
                .OrderByDescending(d => d.CriadoEm)
                .ToListAsync();

            return Ok(documentos);
        }

        [HttpGet("organizado")]
        public async Task<IActionResult> GetOrganizado()
        {
            var docs = await _context.Documentos
                .Include(d => d.Divisao)
                .Include(d => d.Afastamento)
                    .ThenInclude(a => a.Ementario)
                .ToListAsync();

            var resultado = docs
                .GroupBy(d => new { d.CriadoEm.Year, d.CriadoEm.Month })
                .Select(m => new
                {
                    ano = m.Key.Year,
                    mes = m.Key.Month,
                    tipos = m.GroupBy(d => d.Afastamento!.Tipo)
                             .Select(t => new
                             {
                                 tipo = t.Key.ToString(),
                                 documentos = t.Select(d => new
                                 {
                                     id = d.Id,
                                     nomeOriginal = d.NomeOriginal,
                                     criadoEm = d.CriadoEm,
                                     tamanho = d.Tamanho,
                                     divisaoNome = d.Divisao != null
                                         ? d.Divisao.Nome
                                         : null,
                                     ementaResumo = d.Afastamento!.Ementario != null
                                         ? d.Afastamento.Ementario.EmentaResumo
                                         : null,

                                     numeroSei = d.Afastamento?.Ementario?.NumeroSei
                                 })
                             })
                });

            return Ok(resultado);
        }

        [Authorize]
        [HttpGet("{id}/anonimizado")]
        public IActionResult DownloadAnonimizado(Guid id)
        {
            return NotFound("Funcionalidade em desenvolvimento");
        }
    }
}
