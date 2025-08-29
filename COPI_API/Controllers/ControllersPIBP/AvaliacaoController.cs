using COPI_API.DTOs;
using COPI_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COPI_API.Controllers.ControllersPIBP
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AvaliacaoService _avaliacaoService;

        public AvaliacaoController(AvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        // GET: api/Avaliacao/calcular/8/1
        [HttpGet("calcular/{unidadeKpiId}/{cicloId}")]
        [Authorize]
        public async Task<ActionResult<ResultadoAvaliacaoDTO>> Calcular(int unidadeKpiId, int cicloId)
        {
            var resultado = await _avaliacaoService.CalcularIMPIBP(unidadeKpiId, cicloId, salvar: false);
            if (resultado == null)
                return NotFound("Nenhum resultado encontrado para a unidade e ciclo informados.");

            return Ok(resultado);
        }

        // POST: api/Avaliacao/calcular/8/1
        [HttpPost("calcular/{unidadeKpiId}/{cicloId}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<ActionResult<ResultadoAvaliacaoDTO>> CalcularESalvar(int unidadeKpiId, int cicloId)
        {
            var resultado = await _avaliacaoService.CalcularIMPIBP(unidadeKpiId, cicloId, salvar: true);
            if (resultado == null)
                return BadRequest("Erro ao calcular e salvar a avaliação.");

            return Ok(resultado);
        }

        // GET: api/Avaliacao/ciclo/1
        [HttpGet("ciclo/{cicloId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ResultadoAvaliacaoDTO>>> ObterResultadosPorCiclo(int cicloId)
        {
            // Busca todas as unidadesKPI que têm resultados no ciclo
            var unidadesKpi = await _avaliacaoService.GetUnidadesKPIComResultadosDoCiclo(cicloId);
            if (unidadesKpi == null || !unidadesKpi.Any())
                return NotFound("Nenhuma unidade com resultados encontrada para este ciclo.");

            var resultados = new List<ResultadoAvaliacaoDTO>();

            foreach (var unidadeKpi in unidadesKpi)
            {
                var resultado = await _avaliacaoService.CalcularIMPIBP(unidadeKpi.Id, cicloId, salvar: false);
                if (resultado != null)
                {
                    resultados.Add(resultado);
                }
            }

            var mediaGeral = resultados.Average(r => r.NotaIMPIBP);

            return Ok(new
            {
                mediaGeral = Math.Round(mediaGeral, 2),
                resultados
            });
        }
        // GET: api/Avaliacao/ciclo/{cicloId}/eixos
        [HttpGet("ciclo/{cicloId}/eixos")]
        [Authorize]
        public async Task<ActionResult<object>> ObterResultadosPorEixo(int cicloId)
        {
            var dados = await _avaliacaoService.ObterResultadosPorEixo(cicloId);
            return Ok(dados);
        }
    }
}
