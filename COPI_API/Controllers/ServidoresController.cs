// Controller ASP.NET Core para Servidor
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COPI_API.Models;
using COPI_API.Models.DTO;

namespace COPI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServidoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServidoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetServidores()
        {
            var servidores = await _context.Servidores
                .Include(s => s.Divisao)
                .Select(s => new ServidorDTO
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Email = s.Email,
                    DivisaoId = s.DivisaoId,
                    Celular = s.Celular,
                    Status = s.Status,
                    CargoOuFuncao = s.CargoOuFuncao,
                    PontoSei = s.PontoSei,
                    ChefiaImediata = s.ChefiaImediata,
                    RF = s.RF,
                    HorarioEntrada = s.HorarioEntrada,
                    HorarioSaida = s.HorarioSaida,
                    NomeDivisao = s.Divisao.Nome
                })
                .ToListAsync();

            return Ok(servidores);
        }

        [HttpPost]
        public async Task<ActionResult<Servidor>> PostServidor(Servidor servidor)
        {
            _context.Servidores.Add(servidor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServidores), new { id = servidor.Id }, servidor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServidor(int id, Servidor servidor)
        {
            if (id != servidor.Id)
                return BadRequest();

            _context.Entry(servidor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Servidores.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServidor(int id)
        {
            var servidor = await _context.Servidores.FindAsync(id);
            if (servidor == null)
                return NotFound();

            _context.Servidores.Remove(servidor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
