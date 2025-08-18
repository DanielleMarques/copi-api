using COPI_API.Models.AdminEntities;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
namespace COPI_API.Controllers.ControllersAdmin
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UnidadesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unidade>>> Get() => await _context.Unidades.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Unidade>> GetById(int id)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            return unidade == null ? NotFound() : Ok(unidade);
        }

        [HttpPost]
        public async Task<ActionResult<Unidade>> Post(Unidade unidade)
        {
            _context.Unidades.Add(unidade);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = unidade.Id }, unidade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Unidade unidade)
        {
            if (id != unidade.Id) return BadRequest();

            _context.Entry(unidade).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Unidades.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null) return NotFound();

            _context.Unidades.Remove(unidade);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
