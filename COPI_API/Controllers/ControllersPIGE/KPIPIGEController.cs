using COPI_API.Models.PIGEEntities;
using COPI_API.Models.PIGEEntities.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace COPI_API.Controllers.PIGE
{
    [ApiController]
    [Route("api/[controller]")]
    public class KPIPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;
        public KPIPIGEController(AppDbContext context) => _context = context;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<KPIListPIGEDTO>>> Get()
        {
            var kpis = await _context.KPIsPIGE
                .Include(k => k.Eixo)
                .Include(k => k.Nivel)
                .ToListAsync();
            return Ok(kpis.Select(k => new KPIListPIGEDTO
            {
                Id = k.Id,
                Nome = k.Nome ?? string.Empty,
                Pontuacao = k.Pontuacao,
                EixoNome = k.Eixo != null ? k.Eixo.Nome ?? string.Empty : string.Empty,
                NivelNome = k.Nivel != null ? k.Nivel.Nome ?? string.Empty : string.Empty
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<KPIPIGE>> GetById(int id)
        {
            var kpi = await _context.KPIsPIGE
                .Include(k => k.Eixo)
                .Include(k => k.Nivel)
                .FirstOrDefaultAsync(k => k.Id == id);
            if (kpi == null)
                return NotFound();
            return Ok(kpi);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<ActionResult<KPIPIGE>> Post(KPIPIGEDTO kpiDto)
        {
            var nivel = await _context.NiveisPIGE.FindAsync(kpiDto.NivelId);
            var eixo = await _context.EixosPIGE.FindAsync(kpiDto.EixoId);
            if (nivel == null || eixo == null)
                return BadRequest("Nível ou Eixo inválido.");
            var kpi = new KPIPIGE
            {
                Nome = kpiDto.Nome,
                Nivel = nivel,
                Eixo = eixo,
                Pontuacao = 0
            };
            _context.KPIsPIGE.Add(kpi);
            await _context.SaveChangesAsync();
            await RecalcularPontuacoes(eixo.Id, nivel.Id);
            return CreatedAtAction(nameof(GetById), new { id = kpi.Id }, kpi);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> Put(int id, KPIPIGEDTO kpiDto)
        {
            var kpi = await _context.KPIsPIGE
                .Include(k => k.Nivel)
                .Include(k => k.Eixo)
                .FirstOrDefaultAsync(k => k.Id == id);
            if (kpi == null)
                return NotFound();
            
            var novoNivel = await _context.NiveisPIGE.FindAsync(kpiDto.NivelId);
            var novoEixo = await _context.EixosPIGE.FindAsync(kpiDto.EixoId);
            if (novoNivel == null || novoEixo == null)
                return BadRequest("Nível ou Eixo inválido.");

            int eixoAntigoId = kpi.Eixo.Id;
            int nivelAntigoId = kpi.Nivel.Id;
            
            kpi.Nome = kpiDto.Nome;
            kpi.Nivel = novoNivel;
            kpi.Eixo = novoEixo;

            var resultadosRelacionados = await _context.ResultadosKPIPIGE
                .Where(r => r.KPIPIGEId == id)
                .ToListAsync();
            foreach (var resultado in resultadosRelacionados)
            {
                resultado.DataRegistro = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            if (eixoAntigoId != novoEixo.Id || nivelAntigoId != novoNivel.Id)
            {
                await RecalcularPontuacoes(eixoAntigoId, nivelAntigoId);
                await RecalcularPontuacoes(novoEixo.Id, novoNivel.Id);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> Delete(int id)
        {
            var kpi = await _context.KPIsPIGE
                .Include(k => k.Eixo)
                .Include(k => k.Nivel)
                .FirstOrDefaultAsync(k => k.Id == id);
            if (kpi == null)
                return NotFound();

            int eixoId = kpi.Eixo.Id;
            int nivelId = kpi.Nivel.Id;

            _context.KPIsPIGE.Remove(kpi);
            await _context.SaveChangesAsync();
            await RecalcularPontuacoes(eixoId, nivelId);

            return NoContent();
        }

        private async Task RecalcularPontuacoes(int eixoId, int nivelId)
        {
            var kpis = await _context.KPIsPIGE
                .Where(k => k.Eixo.Id == eixoId && k.Nivel.Id == nivelId)
                .ToListAsync();

            if (!kpis.Any()) return;

            double novaPontuacao = Math.Round(10.0 / kpis.Count, 2);
            foreach (var kpi in kpis)
            {
                kpi.Pontuacao = novaPontuacao;
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
