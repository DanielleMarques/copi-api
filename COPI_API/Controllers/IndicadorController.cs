using COPI_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COPI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicadorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IndicadorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("calcular/{unidadeId}/{cicloId}")]
        public async Task<ActionResult<double>> CalcularIMPIBP(int unidadeId, int cicloId)
        {
            var ciclo = await _context.Ciclos.FindAsync(cicloId);
            if (ciclo == null)
                return NotFound("Ciclo não encontrado.");

            var resultados = await _context.ResultadosKPI
                .Include(r => r.KPI)
                    .ThenInclude(k => k!.Eixo)
                .Include(r => r.KPI)
                    .ThenInclude(k => k!.Nivel)
                .Include(r => r.UnidadeKPI)
                .Where(r =>
                    r.UnidadeKPI!.UnidadeId == unidadeId &&
                    r.DataRegistro >= ciclo.DataInicio &&
                    r.DataRegistro <= ciclo.DataFim)
                .ToListAsync();

            double np = CalcularNotaNivel(resultados, "Nivel Padronizado");
            double ni = CalcularNotaNivel(resultados, "Nivel Integrado");
            double ng = CalcularNotaNivel(resultados, "Nivel Gerenciado");

            double imPibp = 0.4 * np + 0.3 * ni + 0.3 * ng;

            return Ok(imPibp);
        }

        private double CalcularNotaNivel(IEnumerable<ResultadoKPI> resultados, string nivel)
        {
            var porNivel = resultados.Where(r => r.KPI?.Nivel?.Nome == nivel);

            double CAA = CalcularNotaEixo(porNivel, "Comprometimento da Alta Administração");
            double CIN = CalcularNotaEixo(porNivel, "Cultura para Integridade");
            double GTR = CalcularNotaEixo(porNivel, "Gestão da Transparência");
            double GRI = CalcularNotaEixo(porNivel, "Gestão de Riscos para Integridade");
            double GIP = CalcularNotaEixo(porNivel, "Gestão da Integridade Pública");

            double nota = 0.25 * CAA + 0.15 * CIN + 0.10 * GTR + 0.25 * GRI + 0.25 * GIP;
            return nota;
        }

        private double CalcularNotaEixo(IEnumerable<ResultadoKPI> resultados, string eixo)
        {
            var kpisDoEixo = resultados
                .Where(r => r.KPI?.Eixo?.Nome == eixo);

            if (!kpisDoEixo.Any()) return 0;

            double soma = 0;
            foreach (var r in kpisDoEixo)
            {
                if (r.Status == "SIM" || r.Status == "NAO_APLICAVEL")
                    soma += r.KPI?.Pontuacao ?? 0;
            }

            return soma;
        }
    }
}
