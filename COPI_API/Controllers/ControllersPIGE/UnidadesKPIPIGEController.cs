using COPI_API.Models.PIGEEntities;
using COPI_API.Models.PIGEEntities.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace COPI_API.Controllers.PIGE
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesKPIPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UnidadesKPIPIGEController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UnidadeKPIPIGEOutputDTO>>> Get()
        {
            var unidades = await _context.Set<UnidadeKPIPIGE>()
                .Include(u => u.Unidade)
                .Include(u => u.Resultados!)
                    .ThenInclude(r => r.KPIPIGE)
                        .ThenInclude(k => k.Eixo)
                .Include(u => u.Resultados!)
                    .ThenInclude(r => r.KPIPIGE)
                        .ThenInclude(k => k.Nivel)
                .Include(u => u.Resultados!)
                    .ThenInclude(r => r.CicloPIGE)
                .ToListAsync();

            var result = unidades.Select(u => new UnidadeKPIPIGEOutputDTO
            {
                Id = u.Id,
                Unidade = u.Unidade?.Nome ?? "",
                UnidadeId = u.UnidadeId,
                NomeAbreviado = u.Unidade?.NomeAbreviado ?? "",
                Tipo = u.Unidade?.Tipo ?? "",
                SEI = u.SEI ?? "",
                Resultados = u.Resultados?.Select(r => new ResultadoSimplificadoPIGEDTO
                {
                    Id = r.Id,
                    KPI = r.KPIPIGE?.Nome ?? "",
                    Eixo = r.KPIPIGE?.Eixo?.Nome ?? "",
                    Nivel = r.KPIPIGE?.Nivel?.Nome ?? "",
                    Ciclo = r.CicloPIGE?.Nome ?? "",
                    CicloEncerrado = r.CicloPIGE?.Encerrado ?? false,
                    Status = r.Status ?? "",
                    DataRegistro = r.DataRegistro
                }).ToList() ?? new List<ResultadoSimplificadoPIGEDTO>()
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UnidadeKPIPIGEOutputDTO>> GetById(int id)
        {
            var unidade = await _context.Set<UnidadeKPIPIGE>()
                .Include(u => u.Unidade)
                .Include(u => u.Resultados!)
                    .ThenInclude(r => r.KPIPIGE)
                        .ThenInclude(k => k.Eixo)
                .Include(u => u.Resultados!)
                    .ThenInclude(r => r.KPIPIGE)
                        .ThenInclude(k => k.Nivel)
                .Include(u => u.Resultados!)
                    .ThenInclude(r => r.CicloPIGE)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unidade == null)
                return NotFound();

            var result = new UnidadeKPIPIGEOutputDTO
            {
                Id = unidade.Id,
                Unidade = unidade.Unidade?.Nome ?? "",
                UnidadeId = unidade.UnidadeId,
                NomeAbreviado = unidade.Unidade?.NomeAbreviado ?? "",
                Tipo = unidade.Unidade?.Tipo ?? "",
                SEI = unidade.SEI ?? "",
                Resultados = unidade.Resultados?.Select(r => new ResultadoSimplificadoPIGEDTO
                {
                    Id = r.Id,
                    KPI = r.KPIPIGE?.Nome ?? "",
                    Eixo = r.KPIPIGE?.Eixo?.Nome ?? "",
                    Nivel = r.KPIPIGE?.Nivel?.Nome ?? "",
                    Ciclo = r.CicloPIGE?.Nome ?? "",
                    CicloEncerrado = r.CicloPIGE?.Encerrado ?? false,
                    Status = r.Status ?? "",
                    DataRegistro = r.DataRegistro
                }).ToList() ?? new List<ResultadoSimplificadoPIGEDTO>()
            };

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<ActionResult<UnidadeKPIPIGEOutputDTO>> Post(UnidadeKPIPIGECreateDTO dto)
        {
            var unidade = await _context.Unidades.FindAsync(dto.UnidadeId);

            if (unidade == null)
                return BadRequest("Unidade inválida.");

            var unidadeKPI = new UnidadeKPIPIGE
            {
                UnidadeId = dto.UnidadeId,
                SEI = dto.SEI
            };

            _context.Set<UnidadeKPIPIGE>().Add(unidadeKPI);
            await _context.SaveChangesAsync();

            var result = new UnidadeKPIPIGEOutputDTO
            {
                Id = unidadeKPI.Id,
                Unidade = unidade.Nome,
                UnidadeId = unidade.Id,
                NomeAbreviado = unidade.NomeAbreviado ?? "",
                Tipo = unidade.Tipo ?? "",
                SEI = unidadeKPI.SEI ?? "",
                Resultados = new List<ResultadoSimplificadoPIGEDTO>()
            };

            return CreatedAtAction(nameof(GetById), new { id = unidadeKPI.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> PutUnidadeKPIPIGE(int id, UnidadeKPIPIGECreateDTO dto)
        {
            var unidadeKPI = await _context.Set<UnidadeKPIPIGE>().FindAsync(id);
            if (unidadeKPI == null)
                return NotFound();

            unidadeKPI.UnidadeId = dto.UnidadeId;
            unidadeKPI.SEI = dto.SEI;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> Delete(int id)
        {
            var unidadeKPI = await _context.Set<UnidadeKPIPIGE>()
                .Include(uk => uk.Resultados)
                .FirstOrDefaultAsync(uk => uk.Id == id);

            if (unidadeKPI == null)
                return NotFound();

            _context.Set<ResultadoKPIPIGE>().RemoveRange(unidadeKPI.Resultados);
            _context.Set<UnidadeKPIPIGE>().Remove(unidadeKPI);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
