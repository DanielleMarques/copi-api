using COPI_API.Models;
using COPI_API.Models.DTO;
using COPI_API.Models.MetaEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace COPI_API.Controllers.ControllersMeta
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MetaService _metaService;
        public TarefaController(AppDbContext context)
        {
            _context = context;
            _metaService = new MetaService(_context);
        }

        // GET: api/Tarefa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaOutputDTO>>> GetTarefas()
        {
            var tarefasIds = await _context.Tarefas.Select(t => t.Id).ToListAsync();
            foreach (var tarefaId in tarefasIds)
            {
                await _metaService.AtualizarCascataPorTarefaAsync(tarefaId);
            }
            var tarefas = await _context.Tarefas
                .Include(t => t.AcaoEstrategica)
                    .ThenInclude(a => a.Meta)
                .ToListAsync();
            var result = tarefas.Select(t => new TarefaOutputDTO
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
                Progresso = t.Progresso,
                AcaoEstrategicaId = t.AcaoEstrategicaId,
                NomeMeta = t.AcaoEstrategica?.Meta?.Titulo
            }).ToList();
            return Ok(result);
        }

        // GET: api/Tarefa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaOutputDTO>> GetTarefa(int id)
        {
            await _metaService.AtualizarCascataPorTarefaAsync(id);
            var t = await _context.Tarefas
                .Include(tarefa => tarefa.AcaoEstrategica)
                    .ThenInclude(a => a.Meta)
                .FirstOrDefaultAsync(tarefa => tarefa.Id == id);
            if (t == null)
                return NotFound();
            var result = new TarefaOutputDTO
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
                Progresso = t.Progresso,
                AcaoEstrategicaId = t.AcaoEstrategicaId,
                NomeMeta = t.AcaoEstrategica != null && t.AcaoEstrategica.Meta != null ? t.AcaoEstrategica.Meta.Titulo : null
            };
            return Ok(result);
        }

        // POST: api/Tarefa
        [HttpPost]
        public async Task<ActionResult<TarefaOutputDTO>> PostTarefa(TarefaInputDTO dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Status = dto.Status,
                StatusExecucao = dto.StatusExecucao,
                AcaoEstrategicaId = dto.AcaoEstrategicaId,
                Responsavel = dto.Responsavel,
                Comentario = dto.Comentario,
                AvaliacaoDoc = dto.AvaliacaoDoc,
                Batida = dto.Batida,
                PrazoFinal = dto.PrazoFinal,
                DataCumprimento = dto.DataCumprimento
            };
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            await _metaService.AtualizarProgressoAcoesAsync(tarefa.AcaoEstrategicaId);
            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, new TarefaOutputDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status,
                StatusExecucao = tarefa.StatusExecucao,
                Responsavel = tarefa.Responsavel,
                Comentario = tarefa.Comentario,
                AvaliacaoDoc = tarefa.AvaliacaoDoc,
                Batida = tarefa.Batida,
                PrazoFinal = tarefa.PrazoFinal,
                DataCumprimento = tarefa.DataCumprimento,
                Progresso = tarefa.Progresso,
                AcaoEstrategicaId = tarefa.AcaoEstrategicaId
            });
        }

        // PUT: api/Tarefa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, TarefaInputDTO dto)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
                return NotFound();
            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.Status = dto.Status;
            tarefa.StatusExecucao = dto.StatusExecucao;
            tarefa.AcaoEstrategicaId = dto.AcaoEstrategicaId;
            tarefa.Responsavel = dto.Responsavel;
            tarefa.Comentario = dto.Comentario;
            tarefa.AvaliacaoDoc = dto.AvaliacaoDoc;
            tarefa.Batida = dto.Batida;
            tarefa.PrazoFinal = dto.PrazoFinal;
            tarefa.DataCumprimento = dto.DataCumprimento;
            await _context.SaveChangesAsync();
            // Atualiza toda a cascata após salvar a tarefa
            await _metaService.AtualizarCascataPorTarefaAsync(id);
            return NoContent();
        }

        // DELETE: api/Tarefa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
                return NotFound();
            int acaoEstrategicaId = tarefa.AcaoEstrategicaId;
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            await _metaService.AtualizarProgressoAcoesAsync(acaoEstrategicaId);
            return NoContent();
        }

        private bool TarefaExists(int id) => _context.Tarefas.Any(t => t.Id == id);
    }
}