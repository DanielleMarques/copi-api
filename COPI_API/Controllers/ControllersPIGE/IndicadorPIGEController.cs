using COPI_API.Models.PIGEEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COPI_API.Controllers.ControllersPIGE
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicadorPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;
        public IndicadorPIGEController(AppDbContext context) => _context = context;

        [HttpGet("calcular/{unidadeId}/{cicloId}")]
        public async Task<ActionResult<double>> CalcularIMPIGE(int unidadeId, int cicloId)
        {
            var ciclo = await _context.CiclosPIGE.FindAsync(cicloId);
            if (ciclo == null)
                return NotFound("Ciclo não encontrado.");

            var resultados = await _context.ResultadosKPIPIGE
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Eixo)
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Nivel)
                .Include(r => r.UnidadeKPIPIGE)
                .Where(r =>
                    r.UnidadeKPIPIGE.UnidadeId == unidadeId &&
                    r.DataRegistro >= ciclo.DataInicio &&
                    r.DataRegistro <= ciclo.DataFim)
                .ToListAsync();

            double np = CalcularNotaNivel(resultados, "Nivel Padronizado");
            double ni = CalcularNotaNivel(resultados, "Nivel Integrado");
            double ng = CalcularNotaNivel(resultados, "Nivel Gerenciado");

            double imPige = 0.4 * np + 0.3 * ni + 0.3 * ng;

            return Ok(imPige);
        }

        private double CalcularNotaNivel(IEnumerable<ResultadoKPIPIGE> resultados, string nivel)
        {
            var porNivel = resultados.Where(r => r.KPIPIGE?.Nivel?.Nome == nivel);

            double GPA = CalcularNotaEixo(porNivel, "Gestão de Práticas Ambientais"); // 0.25
            double GPS = CalcularNotaEixo(porNivel, "Gestão de Práticas Sociais"); // 0.15
            double GPG = CalcularNotaEixo(porNivel, "Gestão de Práticas de Governança"); // 0.25
            double GRI = CalcularNotaEixo(porNivel, "Gestão de Riscos para Integridade"); // 0.25
            double GIP = CalcularNotaEixo(porNivel, "Gestão da Integridade Pública"); // 0.1

            double nota = 0.25 * GPA + 0.15 * GPS + 0.25 * GPG + 0.25 * GRI + 0.10 * GIP;
            return nota;
        }

        private double CalcularNotaEixo(IEnumerable<ResultadoKPIPIGE> resultados, string eixo)
        {
            var kpisDoEixo = resultados
                .Where(r => r.KPIPIGE?.Eixo?.Nome == eixo);

            if (!kpisDoEixo.Any()) return 0;

            double soma = 0;
            foreach (var r in kpisDoEixo)
            {
                if (r.Status == "SIM" || r.Status == "NAO_APLICAVEL")
                    soma += r.KPIPIGE?.Pontuacao ?? 0;
            }

            return soma;
        }
    }
}
