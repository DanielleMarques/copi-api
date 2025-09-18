using COPI_API.Models.PIGEEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COPI_API.Controllers.ControllersPIGE
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StatusPIGEController(AppDbContext context) => _context = context;

        [HttpPost("inicializar")]
        public async Task<IActionResult> InicializarStatus()
        {
            var ciclos = await _context.CiclosPIGE.ToListAsync();
            var unidades = await _context.UnidadesKPIPIGE.ToListAsync();

            foreach (var unidade in unidades)
            {
                foreach (var ciclo in ciclos)
                {
                    var existe = await _context.StatusPIGE.AnyAsync(s =>
                        s.UnidadeKPIPIGEId == unidade.Id && s.CicloPIGEId == ciclo.Id);
                    if (!existe)
                    {
                        _context.StatusPIGE.Add(new StatusPIGE
                        {
                            UnidadeKPIPIGEId = unidade.Id,
                            CicloPIGEId = ciclo.Id,
                            Status = StatusProcessoUnidadeKPIpige.EmAnalise
                        });
                    }
                }
            }
            await _context.SaveChangesAsync();
            return Ok("StatusPIGE inicializados com sucesso.");
        }

        [HttpPost("mudar-para-revisao")]
        public async Task<IActionResult> MudarParaRevisao([FromBody] StatusPIGEUpdateDTO dto)
        {
            var status = await _context.StatusPIGE
                .FirstOrDefaultAsync(s => s.UnidadeKPIPIGEId == dto.UnidadeKPIPIGEId && s.CicloPIGEId == dto.CicloPIGEId); 
            status.Status = StatusProcessoUnidadeKPIpige.ParaRevisao;
            await _context.SaveChangesAsync();
            return Ok("Status alterado para 'Para Revisão'.");
        }

        [HttpPost("aprovar")]
        public async Task<IActionResult> Aprovar([FromBody] StatusPIGEUpdateDTO dto)
        {
            var status = await _context.StatusPIGE
                .FirstOrDefaultAsync(s => s.UnidadeKPIPIGEId == dto.UnidadeKPIPIGEId && s.CicloPIGEId == dto.CicloPIGEId);
            
            status.Status = StatusProcessoUnidadeKPIpige.Aprovado;
            await _context.SaveChangesAsync();
            return Ok("Status alterado para 'Aprovado'.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusPIGE>>> GetAllStatus()
        {
            var statusList = await _context.StatusPIGE
                .Include(s => s.UnidadeKPIPIGE)
                .Include(s => s.CicloPIGE)
                .ToListAsync();
            return Ok(statusList);
        }

        [HttpPost("reprovar")]
        public async Task<IActionResult> Reprovar([FromBody] StatusPIGEUpdateDTO dto)
        {
            var status = await _context.StatusPIGE
                .FirstOrDefaultAsync(s => s.UnidadeKPIPIGEId == dto.UnidadeKPIPIGEId && s.CicloPIGEId == dto.CicloPIGEId);
            
            status.Status = StatusProcessoUnidadeKPIpige.EmAnalise;
            await _context.SaveChangesAsync();
            return Ok("Status alterado para 'Em Análise'.");
        }
    }

    public class StatusPIGEUpdateDTO
    {
        public int UnidadeKPIPIGEId { get; set; }
        public int CicloPIGEId { get; set; }
    }
}
