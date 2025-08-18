using COPI_API.Models;
using COPI_API.Models.PIBPEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace COPI_API.Controllers.ControllersPIBP
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusPIBPController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatusPIBPController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("inicializar")]
        public async Task<IActionResult> InicializarStatus()
        {
            var ciclos = await _context.Ciclos.ToListAsync();
            var unidades = await _context.UnidadesKPI.ToListAsync();

            foreach (var unidade in unidades)
            {
                foreach (var ciclo in ciclos)
                {
                    var existe = await _context.StatusPIBP.AnyAsync(s =>
                        s.UnidadeKPIId == unidade.Id && s.CicloId == ciclo.Id);

                    if (!existe)
                    {
                        _context.StatusPIBP.Add(new StatusPIBP
                        {
                            UnidadeKPIId = unidade.Id,
                            CicloId = ciclo.Id,
                            Status = StatusProcessoUnidadeKPI.EmAnalise
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok("StatusPIBP inicializados com sucesso.");
        }

        [HttpPost("mudar-para-revisao")]
        public async Task<IActionResult> MudarParaRevisao([FromBody] StatusUpdateDTO dto)
        {
            var status = await _context.StatusPIBP
                .FirstOrDefaultAsync(s => s.UnidadeKPIId == dto.UnidadeKPIId && s.CicloId == dto.CicloId);

            if (status == null)
                return NotFound("Registro não encontrado.");

            status.Status = StatusProcessoUnidadeKPI.ParaRevisao;
            await _context.SaveChangesAsync();

            return Ok("Status alterado para 'Para Revisão'.");
        }

        [HttpPost("aprovar")]
        //[Authorize(Roles = "Gestor,Admin")]
        public async Task<IActionResult> Aprovar([FromBody] StatusUpdateDTO dto)
        {
            var status = await _context.StatusPIBP
                .FirstOrDefaultAsync(s => s.UnidadeKPIId == dto.UnidadeKPIId && s.CicloId == dto.CicloId);

            if (status == null)
                return NotFound("Registro não encontrado.");

            status.Status = StatusProcessoUnidadeKPI.Aprovado;
            await _context.SaveChangesAsync();

            return Ok("Status alterado para 'Aprovado'.");
        }

        // GET: api/StatusPIBP
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusPIBP>>> GetAllStatus()
        {
            var statusList = await _context.StatusPIBP
                .Include(s => s.UnidadeKPI)
                .Include(s => s.Ciclo)
                .ToListAsync();
            return Ok(statusList);
        }
        [HttpPost("reprovar")]
        public async Task<IActionResult> Reprovar([FromBody] StatusUpdateDTO dto)
        {
            var status = await _context.StatusPIBP
                .FirstOrDefaultAsync(s => s.UnidadeKPIId == dto.UnidadeKPIId && s.CicloId == dto.CicloId);

            if (status == null)
                return NotFound("Registro não encontrado.");

            status.Status = StatusProcessoUnidadeKPI.EmAnalise;
            await _context.SaveChangesAsync();

            return Ok("Status alterado para 'Em Análise'.");
        }
    }

    public class StatusUpdateDTO
    {
        public int UnidadeKPIId { get; set; }
        public int CicloId { get; set; }
    }
}
