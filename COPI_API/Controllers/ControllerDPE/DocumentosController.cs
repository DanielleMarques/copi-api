using COPI_API.Models.DPEEntities;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static COPI_API.Models.DTO.DPEdto;

namespace COPI_API.Controllers.ControllerEmentario
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public DocumentosController(
            IWebHostEnvironment env,
            AppDbContext context)
        {
            _env = env;
            _context = context;
        }

        [Authorize(Roles = "Admin,Gestor,Operador")]
        [HttpPost("{afastamentoId}")]
        public async Task<IActionResult> Upload(
            int afastamentoId,
            [FromForm] UploadDocumentoDto dto)
        {
            if (dto.Arquivo == null || dto.Arquivo.Length == 0)
                return BadRequest("Arquivo inválido");

            var uploadDir = Path.Combine(
                _env.ContentRootPath,
                "Uploads",
                afastamentoId.ToString()
            );

            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var nomeArmazenado =
                $"{Guid.NewGuid()}{Path.GetExtension(dto.Arquivo.FileName)}";

            var filePath = Path.Combine(uploadDir, nomeArmazenado);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Arquivo.CopyToAsync(stream);
            }

            var documento = new Documento
            {
                Id = Guid.NewGuid(),
                AfastamentoId = afastamentoId,
                NomeOriginal = dto.Arquivo.FileName,
                NomeArmazenado = nomeArmazenado,
                CaminhoArquivo = filePath,
                ContentType = dto.Arquivo.ContentType,
                Tamanho = dto.Arquivo.Length,
                CriadoEm = DateTime.UtcNow,
                CriadoPor = User.Identity?.Name ?? "Sistema",
                Tipo = dto.Tipo,
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return Ok(new { documento.Id });
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

        [Authorize]
        [HttpGet("{id}/anonimizado")]
        public IActionResult DownloadAnonimizado(Guid id)
        {
            return NotFound("Funcionalidade em desenvolvimento");
        }
    }
}
