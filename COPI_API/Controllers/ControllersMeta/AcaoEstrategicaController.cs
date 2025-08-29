using COPI_API.Models.MetaEntities;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using COPI_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace COPI_API.Controllers.ControllersMeta
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcaoEstrategicaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MetaService _metaService;
        public AcaoEstrategicaController(AppDbContext context)
        {
            _context = context;
            _metaService = new MetaService(_context);
        }

        // GET: api/AcaoEstrategica
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AcaoEstrategicaOutputDTO>>> GetAcoesEstrategicas()
        {
            var acoesIds = await _context.AcoesEstrategicas.Select(a => a.Id).ToListAsync();
            foreach (var acaoId in acoesIds)
            {
                await _metaService.AtualizarCascataPorAcaoAsync(acaoId);
            }
            var acoes = await _context.AcoesEstrategicas
                .Include(a => a.Meta)
                .Include(a => a.Tarefas)
                .ToListAsync();
            var result = acoes.Select(a => new AcaoEstrategicaOutputDTO
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
            }).ToList();
            return Ok(result);
        }

        // GET: api/AcaoEstrategica/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AcaoEstrategicaOutputDTO>> GetAcaoEstrategica(int id)
        {
            await _metaService.AtualizarCascataPorAcaoAsync(id);
            var a = await _context.AcoesEstrategicas
                .Include(a => a.Meta)
                .Include(a => a.Tarefas)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (a == null)
                return NotFound();
            var result = new AcaoEstrategicaOutputDTO
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
            };
            return Ok(result);
        }

        // POST: api/AcaoEstrategica
        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,GestorCFCI,GestorDPE,GestorDTA")]
        public async Task<ActionResult<AcaoEstrategicaOutputDTO>> PostAcaoEstrategica(AcaoEstrategicaInputDTO dto)
        {
            var acao = new AcaoEstrategica
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                MetaId = dto.MetaId,
                AcaoExecutada = dto.AcaoExecutada,
                ResponsavelExecucao = dto.ResponsavelExecucao,
                ResponsavelAprovacao = dto.ResponsavelAprovacao,
                Progresso = dto.Progresso,
                Comentarios = dto.Comentarios,
                DocumentosAnalise = dto.DocumentosAnalise,
                Evidencia = dto.Evidencia,
                DocumentoValidado = dto.DocumentoValidado,
                PrazoFinal = dto.PrazoFinal,
                DataCumprimento = dto.DataCumprimento,
                Concluido = dto.Concluido,
                Status = dto.Status,
                Batida = dto.Batida
            };
            _context.AcoesEstrategicas.Add(acao);
            await _context.SaveChangesAsync();
            await _metaService.AtualizarProgressoMetaAsync(acao.MetaId);
            return CreatedAtAction(nameof(GetAcaoEstrategica), new { id = acao.Id }, new AcaoEstrategicaOutputDTO
            {
                Id = acao.Id,
                Titulo = acao.Titulo,
                Descricao = acao.Descricao,
                MetaId = acao.MetaId,
                AcaoExecutada = acao.AcaoExecutada,
                ResponsavelExecucao = acao.ResponsavelExecucao,
                ResponsavelAprovacao = acao.ResponsavelAprovacao,
                Progresso = acao.Progresso,
                Comentarios = acao.Comentarios,
                DocumentosAnalise = acao.DocumentosAnalise,
                Evidencia = acao.Evidencia,
                DocumentoValidado = acao.DocumentoValidado,
                PrazoFinal = acao.PrazoFinal,
                DataCumprimento = acao.DataCumprimento,
                Concluido = acao.Concluido,
                Status = acao.Status,
                Batida = acao.Batida,
                Tarefas = new List<TarefaOutputDTO>()
            });
        }

        // PUT: api/AcaoEstrategica/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,GestorCFCI,GestorDPE,GestorDTA")]
        public async Task<IActionResult> PutAcaoEstrategica(int id, AcaoEstrategicaInputDTO dto)
        {
            var acao = await _context.AcoesEstrategicas.FindAsync(id);
            if (acao == null)
                return NotFound();
            acao.Titulo = dto.Titulo;
            acao.Descricao = dto.Descricao;
            acao.MetaId = dto.MetaId;
            acao.AcaoExecutada = dto.AcaoExecutada;
            acao.ResponsavelExecucao = dto.ResponsavelExecucao;
            acao.ResponsavelAprovacao = dto.ResponsavelAprovacao;
            acao.Progresso = dto.Progresso;
            acao.Comentarios = dto.Comentarios;
            acao.DocumentosAnalise = dto.DocumentosAnalise;
            acao.Evidencia = dto.Evidencia;
            acao.DocumentoValidado = dto.DocumentoValidado;
            acao.PrazoFinal = dto.PrazoFinal;
            acao.DataCumprimento = dto.DataCumprimento;
            acao.Concluido = dto.Concluido;
            acao.Status = dto.Status;
            acao.Batida = dto.Batida;
            await _context.SaveChangesAsync();
            await _metaService.AtualizarProgressoMetaAsync(acao.MetaId);
            return NoContent();
        }

        // DELETE: api/AcaoEstrategica/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,GestorCFCI,GestorDPE,GestorDTA")]
        public async Task<IActionResult> DeleteAcaoEstrategica(int id)
        {
            var acao = await _context.AcoesEstrategicas.FindAsync(id);
            if (acao == null)
                return NotFound();
            int metaId = acao.MetaId;
            _context.AcoesEstrategicas.Remove(acao);
            await _context.SaveChangesAsync();
            await _metaService.AtualizarProgressoMetaAsync(metaId);
            return NoContent();
        }

        // GET: api/AcaoEstrategica/por-meta/{metaId}
        [HttpGet("por-meta/{metaId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AcaoEstrategicaOutputDTO>>> GetAcoesPorMeta(int metaId)
        {
            var acoes = await _context.AcoesEstrategicas
                .Where(a => a.MetaId == metaId)
                .Include(a => a.Tarefas)
                .ToListAsync();
            var result = acoes.Select(a => new AcaoEstrategicaOutputDTO
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
            }).ToList();
            return Ok(result);
        }

        private bool AcaoEstrategicaExists(int id) => _context.AcoesEstrategicas.Any(a => a.Id == id);
    }
}
