using COPI_API.Models;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COPI_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KPIsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KPIsController(AppDbContext context) => _context = context;

        // GET: api/KPIs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KPIListDto>>> Get()
        {
            return await _context.KPIs
                .Include(k => k.Eixo)
                .Include(k => k.Nivel)
                .Select(k => new KPIListDto
                {
                    Id = k.Id,
                    Nome = k.Nome,
                    Pontuacao = k.Pontuacao,
                    EixoNome = k.Eixo.Nome,
                    NivelNome = k.Nivel.Nome
                })
                .ToListAsync();
        }

        // POST: api/KPIs
        [HttpPost]
        public async Task<ActionResult<KPI>> Post(KPIDto kpiDto)
        {
            var nivel = await _context.Niveis.FindAsync(kpiDto.NivelId);
            var eixo = await _context.Eixos.FindAsync(kpiDto.EixoId);

            if (nivel == null || eixo == null)
                return BadRequest("Nível ou Eixo inválido.");

            var kpi = new KPI
            {
                Nome = kpiDto.Nome,
                Nivel = nivel,
                Eixo = eixo,
                Pontuacao = 0
            };

            _context.KPIs.Add(kpi);
            await _context.SaveChangesAsync();

            await RecalcularPontuacoes(eixo.Id, nivel.Id);

            return CreatedAtAction(nameof(Get), new { id = kpi.Id }, kpi);
        }


        // PUT: api/KPIs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, KPIDto kpiDto)
        {
            var kpi = await _context.KPIs
                .Include(k => k.Nivel)
                .Include(k => k.Eixo)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kpi == null)
                return NotFound();

            var novoNivel = await _context.Niveis.FindAsync(kpiDto.NivelId);
            var novoEixo = await _context.Eixos.FindAsync(kpiDto.EixoId);

            if (novoNivel == null || novoEixo == null)
                return BadRequest("Nível ou Eixo inválido.");

            int eixoAntigoId = kpi.Eixo.Id;
            int nivelAntigoId = kpi.Nivel.Id;

            // Atualiza dados do KPI
            kpi.Nome = kpiDto.Nome;
            kpi.Nivel = novoNivel;
            kpi.Eixo = novoEixo;

            // Atualiza DataRegistro dos ResultadosKPI relacionados
            var resultadosRelacionados = await _context.ResultadosKPI
                .Where(r => r.KPIId == id)
                .ToListAsync();

            foreach (var resultado in resultadosRelacionados)
            {
                resultado.DataRegistro = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            if (eixoAntigoId != novoEixo.Id || nivelAntigoId != novoNivel.Id)
                await RecalcularPontuacoes(eixoAntigoId, nivelAntigoId);

            await RecalcularPontuacoes(novoEixo.Id, novoNivel.Id);

            return NoContent();
        }


        // DELETE: api/KPIs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kpi = await _context.KPIs
                .Include(k => k.Eixo)
                .Include(k => k.Nivel)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kpi == null)
                return NotFound();

            int eixoId = kpi.Eixo.Id;
            int nivelId = kpi.Nivel.Id;

            _context.KPIs.Remove(kpi);
            await _context.SaveChangesAsync();

            await RecalcularPontuacoes(eixoId, nivelId);

            return NoContent();
        }

        // Método auxiliar: recalcula as pontuações de um grupo de KPIs com mesmo eixo e nível
        private async Task RecalcularPontuacoes(int eixoId, int nivelId)
        {
            var kpis = await _context.KPIs
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




