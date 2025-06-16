using COPI_API.Models;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COPI_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesKPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UnidadesKPIController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadeKPIOutputDTO>>> Get()
        {
            var unidades = await _context.UnidadesKPI
                .Include(u => u.Unidade)
                .Include(u => u.Servidor)
                .Include(u => u.Resultados!)!
                    .ThenInclude(r => r.KPI)!.ThenInclude(k => k.Eixo)
                .Include(u => u.Resultados!)!
                    .ThenInclude(r => r.KPI)!.ThenInclude(k => k.Nivel)
                .Include(u => u.Resultados!)!
                    .ThenInclude(r => r.Ciclo)
                .ToListAsync();

            var result = unidades.Select(u => new UnidadeKPIOutputDTO
            {
                Id = u.Id,
                Unidade = u.Unidade?.Nome ?? "",
                UnidadeId = u.UnidadeId,
                NomeAbreviado = u.Unidade?.NomeAbreviado ?? "",
                Servidor = u.Servidor?.Nome ?? "",
                ServidorId = u.ServidorId,
                SEI = u.SEI ?? "", // ← Aqui incluído
                Resultados = u.Resultados!.Select(r => new ResultadoSimplificadoDTO
                {
                    Id = r.Id,
                    KPI = r.KPI?.Nome ?? "",
                    Eixo = r.KPI?.Eixo?.Nome ?? "",
                    Nivel = r.KPI?.Nivel?.Nome ?? "",
                    Ciclo = r.Ciclo?.Nome ?? "",
                    CicloEncerrado = r.Ciclo?.Encerrado ?? false,
                    Status = r.Status ?? "",
                    DataRegistro = r.DataRegistro
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UnidadeKPIOutputDTO>> GetById(int id)
        {
            var unidade = await _context.UnidadesKPI
                .Include(u => u.Unidade)
                .Include(u => u.Servidor)
                .Include(u => u.Resultados!)!
                    .ThenInclude(r => r.KPI)!.ThenInclude(k => k.Eixo)
                .Include(u => u.Resultados!)!
                    .ThenInclude(r => r.KPI)!.ThenInclude(k => k.Nivel)
                .Include(u => u.Resultados!)!
                    .ThenInclude(r => r.Ciclo)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unidade == null)
                return NotFound();

            var result = new UnidadeKPIOutputDTO
            {
                Id = unidade.Id,
                Unidade = unidade.Unidade?.Nome ?? "",
                UnidadeId = unidade.UnidadeId,
                NomeAbreviado = unidade.Unidade?.NomeAbreviado ?? "",
                Servidor = unidade.Servidor?.Nome ?? "",
                ServidorId = unidade.ServidorId,
                SEI = unidade.SEI ?? "", // ← Aqui incluído
                Resultados = unidade.Resultados!.Select(r => new ResultadoSimplificadoDTO
                {
                    Id = r.Id,
                    KPI = r.KPI?.Nome ?? "",
                    Eixo = r.KPI?.Eixo?.Nome ?? "",
                    Nivel = r.KPI?.Nivel?.Nome ?? "",
                    Ciclo = r.Ciclo?.Nome ?? "",
                    CicloEncerrado = r.Ciclo?.Encerrado ?? false,
                    Status = r.Status ?? "",
                    DataRegistro = r.DataRegistro
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UnidadeKPIOutputDTO>> Post(UnidadeKPICreateDTO dto)
        {
            var unidade = await _context.Unidades.FindAsync(dto.UnidadeId);
            var servidor = await _context.Servidores.FindAsync(dto.ServidorId);

            if (unidade == null || servidor == null)
                return BadRequest("Unidade ou Servidor inválido.");

            var unidadeKPI = new UnidadeKPI
            {
                UnidadeId = dto.UnidadeId,
                ServidorId = dto.ServidorId,
                SEI = dto.SEI // ← Aqui incluído
            };

            _context.UnidadesKPI.Add(unidadeKPI);
            await _context.SaveChangesAsync();

            var result = new UnidadeKPIOutputDTO
            {
                Id = unidadeKPI.Id,
                Unidade = unidade.Nome,
                UnidadeId = unidade.Id,
                NomeAbreviado = unidade.NomeAbreviado ?? "",
                Servidor = servidor.Nome,
                ServidorId = servidor.Id,
                SEI = unidadeKPI.SEI ?? "",
                Resultados = new List<ResultadoSimplificadoDTO>()
            };

            return CreatedAtAction(nameof(GetById), new { id = unidadeKPI.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidadeKPI(int id, UnidadeKPICreateDTO dto)
        {
            var unidadeKPI = await _context.UnidadesKPI.FindAsync(id);
            if (unidadeKPI == null)
                return NotFound();

            unidadeKPI.UnidadeId = dto.UnidadeId;
            unidadeKPI.ServidorId = dto.ServidorId;
            unidadeKPI.SEI = dto.SEI;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var unidadeKPI = await _context.UnidadesKPI
                .Include(uk => uk.Resultados)
                .FirstOrDefaultAsync(uk => uk.Id == id);

            if (unidadeKPI == null)
                return NotFound();

            // Remover resultados associados
            _context.ResultadosKPI.RemoveRange(unidadeKPI.Resultados);

            // Remover a unidade KPI
            _context.UnidadesKPI.Remove(unidadeKPI);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
