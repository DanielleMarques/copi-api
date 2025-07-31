using COPI_API.Models;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace COPI_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MetaService _metaService;
        public MetaController(AppDbContext context)
        {
            _context = context;
            _metaService = new MetaService(_context);
        }

        // GET: api/Meta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetaOutputDTO>>> GetMetas()
        {
            var metasIds = await _context.Metas.Select(m => m.Id).ToListAsync();
            foreach (var metaId in metasIds)
            {
                await _metaService.AtualizarProgressoMetaAsync(metaId);
            }
            var metas = await _context.Metas
                .Include(m => m.AcoesEstrategicas)
                    .ThenInclude(a => a.Tarefas)
                .ToListAsync();
            var result = metas.Select(m => new MetaOutputDTO
            {
                Id = m.Id,
                Titulo = m.Titulo,
                Descricao = m.Descricao,
                Status = m.Status,
                Tipo = m.Tipo,
                Setor = m.Setor,
                DivisaoId = m.DivisaoId,
                OrigemMeta = m.OrigemMeta,
                DataCumprimento = m.DataCumprimento,
                DataInic = m.DataInic,
                DataFim = m.DataFim,
                Progresso = m.Progresso,
                AcoesEstrategicas = m.AcoesEstrategicas?.Select(a => new AcaoEstrategicaOutputDTO
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Descricao = a.Descricao,
                    MetaId = a.MetaId,
                    AcaoExecutada = a.AcaoExecutada,
                    ResponsavelExecucao = a.ResponsavelExecucao,
                    ResponsavelAprovacao = a.ResponsavelAprovacao,
                    Progresso = a.Progresso,
                    Comentarios = a.Comentarios,
                    DocumentosAnalise = a.DocumentosAnalise,
                    Evidencia = a.Evidencia,
                    DocumentoValidado = a.DocumentoValidado,
                    PrazoFinal = a.PrazoFinal,
                    DataCumprimento = a.DataCumprimento,
                    Concluido = a.Concluido,
                    Status = a.Status,
                    Batida = a.Batida,
                    Tarefas = a.Tarefas?.Select(t => new TarefaOutputDTO
                    {
                        Id = t.Id,
                        Titulo = t.Titulo,
                        Descricao = t.Descricao,
                        Status = t.Status,
                        StatusExecucao = t.StatusExecucao,
                        Responsavel = t.Responsavel,
                        Comentario = t.Comentario,
                        AvaliacaoDoc = t.AvaliacaoDoc,
                        Batida = t.Batida,
                        PrazoFinal = t.PrazoFinal,
                        DataCumprimento = t.DataCumprimento,
                        Progresso = t.Progresso
                    }).ToList()
                }).ToList()
            }).ToList();
            return Ok(result);
        }

        // GET: api/Meta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MetaOutputDTO>> GetMeta(int id)
        {
            await _metaService.AtualizarProgressoMetaAsync(id);
            var m = await _context.Metas
                .Include(meta => meta.AcoesEstrategicas)
                    .ThenInclude(a => a.Tarefas)
                .FirstOrDefaultAsync(meta => meta.Id == id);
            if (m == null)
                return NotFound();
            var result = new MetaOutputDTO
            {
                Id = m.Id,
                Titulo = m.Titulo,
                Descricao = m.Descricao,
                Status = m.Status,
                Tipo = m.Tipo,
                Setor = m.Setor,
                DivisaoId = m.DivisaoId,
                OrigemMeta = m.OrigemMeta,
                DataCumprimento = m.DataCumprimento,
                DataInic = m.DataInic,
                DataFim = m.DataFim,
                Progresso = m.Progresso,
                AcoesEstrategicas = m.AcoesEstrategicas?.Select(a => new AcaoEstrategicaOutputDTO
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Descricao = a.Descricao,
                    MetaId = a.MetaId,
                    AcaoExecutada = a.AcaoExecutada,
                    ResponsavelExecucao = a.ResponsavelExecucao,
                    ResponsavelAprovacao = a.ResponsavelAprovacao,
                    Progresso = a.Progresso,
                    Comentarios = a.Comentarios,
                    DocumentosAnalise = a.DocumentosAnalise,
                    Evidencia = a.Evidencia,
                    DocumentoValidado = a.DocumentoValidado,
                    PrazoFinal = a.PrazoFinal,
                    DataCumprimento = a.DataCumprimento,
                    Concluido = a.Concluido,
                    Status = a.Status,
                    Batida = a.Batida,
                    Tarefas = a.Tarefas?.Select(t => new TarefaOutputDTO
                    {
                        Id = t.Id,
                        Titulo = t.Titulo,
                        Descricao = t.Descricao,
                        Status = t.Status,
                        StatusExecucao = t.StatusExecucao,
                        Responsavel = t.Responsavel,
                        Comentario = t.Comentario,
                        AvaliacaoDoc = t.AvaliacaoDoc,
                        Batida = t.Batida,
                        PrazoFinal = t.PrazoFinal,
                        DataCumprimento = t.DataCumprimento,
                        Progresso = t.Progresso
                    }).ToList()
                }).ToList()
            };
            return Ok(result);
        }

        // POST: api/Meta
        [HttpPost]
        public async Task<ActionResult<MetaOutputDTO>> PostMeta(MetaInputDTO dto)
        {
            var meta = new Meta
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Status = dto.Status,
                Tipo = dto.Tipo,
                Setor = dto.Setor,
                DivisaoId = dto.DivisaoId,
                OrigemMeta = dto.OrigemMeta,
                DataCumprimento = dto.DataCumprimento,
                DataInic = dto.DataInic,
                DataFim = dto.DataFim
            };
            _context.Metas.Add(meta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMeta), new { id = meta.Id }, new MetaOutputDTO
            {
                Id = meta.Id,
                Titulo = meta.Titulo,
                Descricao = meta.Descricao,
                Status = meta.Status,
                Tipo = meta.Tipo,
                Setor = meta.Setor,
                DivisaoId = meta.DivisaoId,
                OrigemMeta = meta.OrigemMeta,
                DataCumprimento = meta.DataCumprimento,
                DataInic = meta.DataInic,
                DataFim = meta.DataFim,
                Progresso = meta.Progresso,
                AcoesEstrategicas = new List<AcaoEstrategicaOutputDTO>()
            });
        }

        // PUT: api/Meta/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeta(int id, MetaInputDTO dto)
        {
            var meta = await _context.Metas.FindAsync(id);
            if (meta == null)
                return NotFound();
            meta.Titulo = dto.Titulo;
            meta.Descricao = dto.Descricao;
            meta.Status = dto.Status;
            meta.Tipo = dto.Tipo;
            meta.Setor = dto.Setor;
            meta.DivisaoId = dto.DivisaoId;
            meta.OrigemMeta = dto.OrigemMeta;
            meta.DataCumprimento = dto.DataCumprimento;
            meta.DataInic = dto.DataInic;
            meta.DataFim = dto.DataFim;
            await _context.SaveChangesAsync();
            await _metaService.AtualizarProgressoMetaAsync(meta.Id);
            return NoContent();
        }

        // DELETE: api/Meta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeta(int id)
        {
            var meta = await _context.Metas.FindAsync(id);
            if (meta == null)
                return NotFound();
            _context.Metas.Remove(meta);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Meta/media-progresso
        [HttpGet("media-progresso")]
        public async Task<ActionResult<double>> GetMediaProgressoMetas()
        {
            var metas = await _context.Metas.ToListAsync();
            if (metas == null || metas.Count == 0)
                return Ok(0);
            double media = metas.Average(m => m.Progresso);
            return Ok(Math.Round(media, 2));
        }

        // GET: api/Meta/status-count
        [HttpGet("status-count")]
        public async Task<ActionResult<object>> GetMetasStatusCount()
        {
            var statusList = new[] { "Pendente", "Em andamento", "Concluída", "Descontinuado" };
            var metas = await _context.Metas.ToListAsync();
            var result = statusList.ToDictionary(
                status => status,
                status => metas.Count(m => m.Status == status)
            );
            return Ok(result);
        }

        // GET: api/Meta/media-por-divisao
        [HttpGet("media-por-divisao")]
        public async Task<ActionResult<object>> GetMediaProgressoPorDivisao()
        {
            var metas = await _context.Metas.ToListAsync();
            if (metas == null || metas.Count == 0)
                return Ok(new Dictionary<string, double>());

            var divisoes = await _context.Divisoes.ToListAsync();
            var result = divisoes.ToDictionary(
                d => d.Nome ?? $"Divisão {d.Id}",
                d => {
                    var metasDivisao = metas.Where(m => m.DivisaoId == d.Id).ToList();
                    return metasDivisao.Count > 0 ? Math.Round(metasDivisao.Average(m => m.Progresso), 2) : 0;
                }
            );
            return Ok(result);
        }

        // GET: api/Meta/media-por-tipo
        [HttpGet("media-por-tipo")]
        public async Task<ActionResult<object>> GetMediaProgressoPorTipo()
        {
            var metas = await _context.Metas.ToListAsync();
            if (metas == null || metas.Count == 0)
                return Ok(new Dictionary<string, double>());

            var tipos = metas.Select(m => m.Tipo).Distinct().Where(t => !string.IsNullOrEmpty(t)).ToList();
            var result = tipos.ToDictionary(
                tipo => tipo!,
                tipo => {
                    var metasTipo = metas.Where(m => m.Tipo == tipo).ToList();
                    return metasTipo.Count > 0 ? Math.Round(metasTipo.Average(m => m.Progresso), 2) : 0;
                }
            );
            return Ok(result);
        }

        private bool MetaExists(int id) => _context.Metas.Any(m => m.Id == id);
    }
}
