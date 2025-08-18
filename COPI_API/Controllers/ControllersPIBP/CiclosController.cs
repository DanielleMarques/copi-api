using COPI_API.Models;
using COPI_API.Models.PIBPEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace COPI_API.Controllers.ControllersPIBP
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiclosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CiclosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ciclo>>> GetCiclos()
        {
            return await _context.Ciclos.OrderByDescending(c => c.DataInicio).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ciclo>> GetCiclo(int id)
        {
            var ciclo = await _context.Ciclos.FindAsync(id);

            if (ciclo == null)
                return NotFound();
            return ciclo;
        }

        [HttpPost]
        public async Task<ActionResult<Ciclo>> PostCiclo(Ciclo ciclo)
        {
            _context.Ciclos.Add(ciclo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCiclo), new { id = ciclo.Id }, ciclo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiclo(int id, Ciclo ciclo)
        {
            if (id != ciclo.Id)
                return BadRequest();

            var cicloExistente = await _context.Ciclos.FindAsync(id);
            if (cicloExistente == null)
                return NotFound();

            cicloExistente.Nome = ciclo.Nome;
            cicloExistente.Encerrado = ciclo.Encerrado;
            cicloExistente.DataInicio = ciclo.DataInicio;
            cicloExistente.DataFim = ciclo.DataFim;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiclo(int id)
        {
            var ciclo = await _context.Ciclos.FindAsync(id);
            if (ciclo == null)
                return NotFound();

            _context.Ciclos.Remove(ciclo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}