using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COPI_API.Models;
using COPI_API.Models.DTO;

namespace COPI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadoKPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResultadoKPIController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoKPIOutputDTO>>> GetResultados()
        {
            var resultados = await _context.ResultadosKPI
                .Include(r => r.KPI)
                .Include(r => r.UnidadeKPI)
                .Include(r => r.Ciclo)
                .ToListAsync();

            return resultados.Select(r => new ResultadoKPIOutputDTO
            {
                Id = r.Id,
                KPI = r.KPI?.Nome ?? "N/A",
                Unidade = r.UnidadeKPI?.Unidade?.Nome ?? "N/A",
                CicloId = r.CicloId,
                Ciclo = r.Ciclo?.Nome ?? "N/A", // Assumindo que Ciclo tem propriedade Nome
                Status = r.Status ?? "N/A",
                Prova = r.Prova ?? "",
                AvaliacaoEscrita = r.AvaliacaoEscrita ?? "",
                DataRegistro = r.DataRegistro
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultadoKPIOutputDTO>> GetResultadoKPI(int id)
        {
            var resultado = await _context.ResultadosKPI
                .Include(r => r.KPI)
                .Include(r => r.UnidadeKPI)
                .Include(r => r.Ciclo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resultado == null)
                return NotFound();

            return new ResultadoKPIOutputDTO
            {
                Id = resultado.Id,
                KPI = resultado.KPI?.Nome ?? "N/A",
                Unidade = resultado.UnidadeKPI?.Unidade?.Nome ?? "N/A",
                CicloId = resultado.CicloId,
                Ciclo = resultado.Ciclo?.Nome ?? "N/A",
                Status = resultado.Status ?? "N/A",
                Prova = resultado.Prova ?? "",
                AvaliacaoEscrita = resultado.AvaliacaoEscrita ?? "",
                DataRegistro = resultado.DataRegistro
            };
        }

        // GET: api/ResultadoKPI/unidade/{unidadeKPIId}/ciclo/{cicloId}
        [HttpGet("unidade/{unidadeKPIId}/ciclo/{cicloId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetResultadosPorUnidadeECiclo(int unidadeKPIId, int cicloId)
        {
            var resultados = await _context.ResultadosKPI
                .Where(r => r.UnidadeKPIId == unidadeKPIId && r.CicloId == cicloId)
                .Include(r => r.KPI).ThenInclude(k => k.Eixo)
                .Include(r => r.KPI).ThenInclude(k => k.Nivel)
                .Include(r => r.UnidadeKPI).ThenInclude(uk => uk.Unidade)
                .Include(r => r.Ciclo)
                .Select(r => new
                {
                    r.Id,
                    r.Status,
                    r.Prova,
                    r.AvaliacaoEscrita,
                    r.DataRegistro,
                    CicloId = r.CicloId,
                    Ciclo = r.Ciclo!.Nome,
                    KPI = new
                    {
                        r.KPI!.Id,
                        r.KPI.Nome,
                        r.KPI.Pontuacao,
                        Eixo = new { Nome = r.KPI.Eixo!.Nome },
                        Nivel = new { Nome = r.KPI.Nivel!.Nome }
                    },
                    UnidadeKPI = new
                    {
                        r.UnidadeKPIId,
                        Nome = r.UnidadeKPI!.Unidade.Nome
                    }
                })
                .ToListAsync();

            return Ok(resultados);
        }


        [HttpPost]
        public async Task<ActionResult<ResultadoKPIOutputDTO>> PostResultadoKPI(ResultadoKPIInputDTO dto)
        {
            var kpi = await _context.KPIs.FindAsync(dto.KPIId);
            var unidadeKPI = await _context.UnidadesKPI.FindAsync(dto.UnidadeKPIId);
            var ciclo = await _context.Ciclos.FindAsync(dto.CicloId);

            if (kpi == null || unidadeKPI == null || ciclo == null)
                return BadRequest("IDs de KPI, UnidadeKPI ou Ciclo inválidos.");

            var resultado = new ResultadoKPI
            {
                KPI = kpi,
                UnidadeKPI = unidadeKPI,
                Ciclo = ciclo,
                Status = dto.Status,
                Prova = dto.Prova,
                AvaliacaoEscrita = dto.AvaliacaoEscrita,
                DataRegistro = DateTime.UtcNow
            };

            _context.ResultadosKPI.Add(resultado);
            await _context.SaveChangesAsync();

            var output = new ResultadoKPIOutputDTO
            {
                Id = resultado.Id,
                KPI = kpi.Nome,
                Unidade = unidadeKPI.Unidade.Nome ?? "",
                CicloId = ciclo.Id,
                Ciclo = ciclo.Nome ?? "",
                Status = resultado.Status ?? "",
                Prova = resultado.Prova ?? "",
                AvaliacaoEscrita = resultado.AvaliacaoEscrita ?? "",
                DataRegistro = resultado.DataRegistro
            };

            return CreatedAtAction(nameof(GetResultadoKPI), new { id = resultado.Id }, output);
        }
        [HttpPost("sincronizar-resultados")]
        public async Task<IActionResult> SincronizarResultados()
        {
            var unidadesKPI = await _context.UnidadesKPI.ToListAsync();
            var kpis = await _context.KPIs.ToListAsync();
            var ciclos = await _context.Ciclos.ToListAsync();

            var resultadosExistentes = await _context.ResultadosKPI
                .Select(r => new { r.UnidadeKPIId, r.KPIId, r.CicloId })
                .ToListAsync();

            var novosResultados = new List<ResultadoKPI>();

            foreach (var unidade in unidadesKPI)
            {
                foreach (var kpi in kpis)
                {
                    foreach (var ciclo in ciclos)
                    {
                        bool jaExiste = resultadosExistentes.Any(r =>
                            r.UnidadeKPIId == unidade.Id &&
                            r.KPIId == kpi.Id &&
                            r.CicloId == ciclo.Id);

                        if (!jaExiste)
                        {
                            novosResultados.Add(new ResultadoKPI
                            {
                                UnidadeKPIId = unidade.Id,
                                KPIId = kpi.Id,
                                CicloId = ciclo.Id,
                                Status = "NAO",
                                Prova = null,
                                AvaliacaoEscrita = null,
                                DataRegistro = DateTime.UtcNow
                            });
                        }
                    }
                }
            }

            if (novosResultados.Any())
            {
                _context.ResultadosKPI.AddRange(novosResultados);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                QuantidadeCriada = novosResultados.Count,
                Mensagem = novosResultados.Count > 0
                    ? "Resultados sincronizados com sucesso."
                    : "Nenhum novo resultado necessário. Tudo já está sincronizado."
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutResultadoKPI(int id, ResultadoKPIInputDTO dto)
        {
            var resultado = await _context.ResultadosKPI
                .Include(r => r.KPI)
                .Include(r => r.UnidadeKPI)
                .Include(r => r.Ciclo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resultado == null)
                return NotFound();

            var kpi = await _context.KPIs.FindAsync(dto.KPIId);
            var unidadeKPI = await _context.UnidadesKPI.FindAsync(dto.UnidadeKPIId);
            var ciclo = await _context.Ciclos.FindAsync(dto.CicloId);

            if (kpi == null || unidadeKPI == null || ciclo == null)
                return BadRequest("IDs de KPI, UnidadeKPI ou Ciclo inválidos.");

            resultado.KPI = kpi;
            resultado.UnidadeKPI = unidadeKPI;
            resultado.Ciclo = ciclo;
            resultado.Status = dto.Status;
            resultado.Prova = dto.Prova;
            resultado.AvaliacaoEscrita = dto.AvaliacaoEscrita;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResultadoKPI(int id)
        {
            var resultado = await _context.ResultadosKPI.FindAsync(id);
            if (resultado == null)
                return NotFound();

            _context.ResultadosKPI.Remove(resultado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}






