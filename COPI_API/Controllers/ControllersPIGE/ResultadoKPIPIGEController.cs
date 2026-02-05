using COPI_API.Models.PIGEEntities;
using COPI_API.Models.PIGEEntities.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace COPI_API.Controllers.ControllersPIGE
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadoKPIPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResultadoKPIPIGEController(AppDbContext context) => _context = context;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ResultadoKPIPIGEOutputDTO>>> GetResultados()
        {
            var resultados = await _context.Set<ResultadoKPIPIGE>()
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Eixo)
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Nivel)
                .Include(r => r.UnidadeKPIPIGE)
                    .ThenInclude(u => u.Unidade)
                .Include(r => r.CicloPIGE)
                .ToListAsync();

            return Ok(resultados.Select(r => new ResultadoKPIPIGEOutputDTO
            {
                Id = r.Id,
                KPI = r.KPIPIGE?.Nome ?? "N/A",
                Unidade = r.UnidadeKPIPIGE?.Unidade?.Nome ?? "N/A",
                CicloId = r.CicloPIGEId,
                Ciclo = r.CicloPIGE?.Nome ?? "N/A",
                Status = r.Status ?? "N/A",
                Prova = r.Prova ?? "",
                AvaliacaoEscrita = r.AvaliacaoEscrita ?? "",
                DataRegistro = r.DataRegistro,
                Eixo = r.KPIPIGE?.Eixo?.Nome ?? "N/A",
                Nivel = r.KPIPIGE?.Nivel?.Nome ?? "N/A",
                CicloEncerrado = r.CicloPIGE?.Encerrado ?? false,
                UltimaAlteracaoPor = r.UltimaAlteracaoPor,
                UltimaAlteracaoEm = r.UltimaAlteracaoEm
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ResultadoKPIPIGEOutputDTO>> GetResultadoKPIPIGE(int id)
        {
            var resultado = await _context.Set<ResultadoKPIPIGE>()
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Eixo)
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Nivel)
                .Include(r => r.UnidadeKPIPIGE)
                    .ThenInclude(u => u.Unidade)
                .Include(r => r.CicloPIGE)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resultado == null)
                return NotFound();

            return Ok(new ResultadoKPIPIGEOutputDTO
            {
                Id = resultado.Id,
                KPI = resultado.KPIPIGE?.Nome ?? "N/A",
                Unidade = resultado.UnidadeKPIPIGE?.Unidade?.Nome ?? "N/A",
                CicloId = resultado.CicloPIGEId,
                Ciclo = resultado.CicloPIGE?.Nome ?? "N/A",
                Status = resultado.Status ?? "N/A",
                Prova = resultado.Prova ?? "",
                AvaliacaoEscrita = resultado.AvaliacaoEscrita ?? "",
                DataRegistro = resultado.DataRegistro,
                Eixo = resultado.KPIPIGE?.Eixo?.Nome ?? "N/A",
                Nivel = resultado.KPIPIGE?.Nivel?.Nome ?? "N/A",
                CicloEncerrado = resultado.CicloPIGE?.Encerrado ?? false,
                UltimaAlteracaoPor = resultado.UltimaAlteracaoPor,
                UltimaAlteracaoEm = resultado.UltimaAlteracaoEm
            });
        }

        [HttpGet("unidade/{unidadeKPIPIGEId}/ciclo/{cicloId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetResultadosPorUnidadeECiclo(int unidadeKPIPIGEId, int cicloId)
        {
            var resultados = await _context.Set<ResultadoKPIPIGE>()
                .Where(r => r.UnidadeKPIPIGEId == unidadeKPIPIGEId && r.CicloPIGEId == cicloId)
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Eixo)
                .Include(r => r.KPIPIGE)
                    .ThenInclude(k => k.Nivel)
                .Include(r => r.UnidadeKPIPIGE)
                    .ThenInclude(uk => uk.Unidade)
                .Include(r => r.CicloPIGE)
                .Select(r => new
                {
                    r.Id,
                    r.Status,
                    r.Prova,
                    r.AvaliacaoEscrita,
                    r.DataRegistro,
                    CicloId = r.CicloPIGEId,
                    Ciclo = r.CicloPIGE.Nome,

                    UltimaAlteracaoPor = r.UltimaAlteracaoPor,
                    UltimaAlteracaoEm = r.UltimaAlteracaoEm,

                    KPI = new
                    {
                        r.KPIPIGE.Id,
                        r.KPIPIGE.Nome,
                        r.KPIPIGE.Pontuacao,
                        Eixo = new { Nome = r.KPIPIGE.Eixo.Nome },
                        Nivel = new { Nome = r.KPIPIGE.Nivel.Nome }
                    },
                    UnidadeKPIPIGE = new
                    {
                        r.UnidadeKPIPIGEId,
                        Nome = r.UnidadeKPIPIGE.Unidade.Nome
                    }
                })
                .ToListAsync();

            return Ok(resultados);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIGE")]
        public async Task<ActionResult<ResultadoKPIPIGEOutputDTO>> PostResultadoKPIPIGE(ResultadoKPIPIGEInputDTO dto)
        {
            // Verifica duplicidade
            var existe = await _context.Set<ResultadoKPIPIGE>().AnyAsync(r =>
                r.UnidadeKPIPIGEId == dto.UnidadeKPIPIGEId &&
                r.KPIPIGEId == dto.KPIPIGEId &&
                r.CicloPIGEId == dto.CicloPIGEId);

            if (existe)
                return BadRequest("Já existe um resultado para esta combinação de Unidade, KPI e Ciclo.");

            var kpi = await _context.Set<KPIPIGE>()
                .Include(k => k.Eixo)
                .Include(k => k.Nivel)
                .FirstOrDefaultAsync(k => k.Id == dto.KPIPIGEId);

            var unidadeKPI = await _context.Set<UnidadeKPIPIGE>()
                .Include(u => u.Unidade)
                .FirstOrDefaultAsync(u => u.Id == dto.UnidadeKPIPIGEId);

            var ciclo = await _context.Set<CicloPIGE>().FindAsync(dto.CicloPIGEId);

            if (kpi == null || unidadeKPI == null || ciclo == null)
                return BadRequest("IDs de KPI, UnidadeKPIPIGE ou Ciclo inválidos.");

            var resultado = new ResultadoKPIPIGE
            {
                KPIPIGEId = kpi.Id,
                UnidadeKPIPIGEId = unidadeKPI.Id,
                CicloPIGEId = ciclo.Id,
                Status = dto.Status,
                Prova = dto.Prova,
                AvaliacaoEscrita = dto.AvaliacaoEscrita,
                DataRegistro = DateTime.UtcNow
            };

            _context.Set<ResultadoKPIPIGE>().Add(resultado);
            await _context.SaveChangesAsync();

            var output = new ResultadoKPIPIGEOutputDTO
            {
                Id = resultado.Id,
                KPI = kpi.Nome ?? "",
                Unidade = unidadeKPI.Unidade?.Nome ?? "",
                CicloId = ciclo.Id,
                Ciclo = ciclo.Nome ?? "",
                Status = resultado.Status ?? "",
                Prova = resultado.Prova ?? "",
                AvaliacaoEscrita = resultado.AvaliacaoEscrita ?? "",
                DataRegistro = resultado.DataRegistro,
                Eixo = kpi.Eixo?.Nome ?? "",
                Nivel = kpi.Nivel?.Nome ?? "",
                CicloEncerrado = ciclo.Encerrado ?? false
            };

            return CreatedAtAction(nameof(GetResultadoKPIPIGE), new { id = resultado.Id }, output);
        }

        [HttpPost("sincronizar-resultados")]
        [Authorize(Roles = "Admin,Gestor, GestorPIBP")]
        public async Task<IActionResult> SincronizarResultados()
        {
            var unidadesKPI = await _context.Set<UnidadeKPIPIGE>().ToListAsync();
            var kpis = await _context.Set<KPIPIGE>().ToListAsync();
            var ciclos = await _context.Set<CicloPIGE>().ToListAsync();

            var resultadosExistentes = await _context.Set<ResultadoKPIPIGE>()
                .Select(r => new { r.UnidadeKPIPIGEId, r.KPIPIGEId, r.CicloPIGEId })
                .ToListAsync();

            var novosResultados = new List<ResultadoKPIPIGE>();

            foreach (var unidade in unidadesKPI)
            {
                foreach (var kpi in kpis)
                {
                    foreach (var ciclo in ciclos)
                    {
                        bool jaExiste = resultadosExistentes.Any(r =>
                            r.UnidadeKPIPIGEId == unidade.Id &&
                            r.KPIPIGEId == kpi.Id &&
                            r.CicloPIGEId == ciclo.Id);

                        if (!jaExiste)
                        {
                            novosResultados.Add(new ResultadoKPIPIGE
                            {
                                UnidadeKPIPIGEId = unidade.Id,
                                KPIPIGEId = kpi.Id,
                                CicloPIGEId = ciclo.Id,
                                Status = null,
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
                _context.Set<ResultadoKPIPIGE>().AddRange(novosResultados);
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
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIGE")]
        public async Task<IActionResult> PutResultadoKPIPIGE(int id, ResultadoKPIPIGEInputDTO dto)
        {
            var resultado = await _context.Set<ResultadoKPIPIGE>().FindAsync(id);
            if (resultado == null)
                return NotFound();

            var kpi = await _context.Set<KPIPIGE>().FindAsync(dto.KPIPIGEId);
            var unidadeKPI = await _context.Set<UnidadeKPIPIGE>().FindAsync(dto.UnidadeKPIPIGEId);
            var ciclo = await _context.Set<CicloPIGE>().FindAsync(dto.CicloPIGEId);

            if (kpi == null || unidadeKPI == null || ciclo == null)
                return BadRequest("IDs de KPI, UnidadeKPIPIGE ou Ciclo inválidos.");

            resultado.KPIPIGEId = kpi.Id;
            resultado.UnidadeKPIPIGEId = unidadeKPI.Id;
            resultado.CicloPIGEId = ciclo.Id;
            resultado.Status = dto.Status;
            resultado.Prova = dto.Prova;
            resultado.AvaliacaoEscrita = dto.AvaliacaoEscrita;

            resultado.UltimaAlteracaoPor =
              User.FindFirst(ClaimTypes.Name)?.Value ??
              User.FindFirst(ClaimTypes.Email)?.Value;

            resultado.UltimaAlteracaoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIGE")]
        public async Task<IActionResult> DeleteResultadoKPIPIGE(int id)
        {
            var resultado = await _context.Set<ResultadoKPIPIGE>().FindAsync(id);
            if (resultado == null)
                return NotFound();

            _context.Set<ResultadoKPIPIGE>().Remove(resultado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
