using COPI_API.Models.PIGEEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace COPI_API.Controllers.PIGE
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoPIGEController : ControllerBase
    {
        private readonly AvaliacaoServicePIGE _avaliacaoService;

        public AvaliacaoPIGEController(AvaliacaoServicePIGE avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        [HttpGet("calcular/{unidadeKpiId}/{cicloId}")]
        [Authorize]
        public async Task<IActionResult> Calcular(int unidadeKpiId, int cicloId)
        {
            var resultado = await _avaliacaoService.CalcularIMPIGE(unidadeKpiId, cicloId, salvar: false);
            if (resultado == null)
                return NotFound("Nenhum resultado encontrado para a unidade e ciclo informados.");

            return Ok(resultado);
        }

        [HttpPost("calcular/{unidadeKpiId}/{cicloId}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> CalcularESalvar(int unidadeKpiId, int cicloId)
        {
            var resultado = await _avaliacaoService.CalcularIMPIGE(unidadeKpiId, cicloId, salvar: true);
            if (resultado == null)
                return BadRequest("Erro ao calcular e salvar a avaliação.");

            return Ok(resultado);
        }

        [HttpGet("ciclo/{cicloId}")]
        [Authorize]
        public async Task<IActionResult> ObterResultadosPorCiclo(int cicloId)
        {
            var unidadesKpi = await _avaliacaoService.GetUnidadesKPIPIGEComResultadosDoCiclo(cicloId);
            if (unidadesKpi == null || !unidadesKpi.Any())
                return NotFound("Nenhuma unidade com resultados encontrada para este ciclo.");

            var resultados = new System.Collections.Generic.List<object>();

            foreach (var unidadeKpi in unidadesKpi)
            {
                var resultado = await _avaliacaoService.CalcularIMPIGE(unidadeKpi.Id, cicloId, salvar: false);
                if (resultado != null)
                {
                    resultados.Add(resultado);
                }
            }

            var mediaGeral = resultados.Count > 0
                ? resultados.Average(r => (double)((dynamic)r).NotaIMPIGE)
                : 0.0;

            return Ok(new
            {
                mediaGeral = System.Math.Round(mediaGeral, 2),
                resultados
            });
        }

        [HttpGet("ciclo/{cicloId}/eixos")]
        [Authorize]
        public async Task<IActionResult> ObterResultadosPorEixo(int cicloId)
        {
            var dados = await _avaliacaoService.ObterResultadosPorEixo(cicloId);
            return Ok(dados);
        }
    }
}
