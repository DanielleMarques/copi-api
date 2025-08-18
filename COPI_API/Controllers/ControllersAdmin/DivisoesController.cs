using COPI_API.Models.AdminEntities;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
namespace COPI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DivisoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Divisoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Divisao>>> GetDivisoes()
        {
            return await _context.Divisoes.ToListAsync();
        }

        // GET: api/Divisoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Divisao>> GetDivisao(int id)
        {
            var divisao = await _context.Divisoes.FindAsync(id);

            if (divisao == null)
                return NotFound();

            return divisao;
        }

        // POST: api/Divisoes
        [HttpPost]
        public async Task<ActionResult<Divisao>> PostDivisao(Divisao divisao)
        {
            _context.Divisoes.Add(divisao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDivisao), new { id = divisao.Id }, divisao);
        }

        // PUT: api/Divisoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivisao(int id, Divisao divisao)
        {
            if (id != divisao.Id)
                return BadRequest("ID da divisão não corresponde.");

            _context.Entry(divisao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DivisaoExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/Divisoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDivisao(int id)
        {
            var divisao = await _context.Divisoes.FindAsync(id);
            if (divisao == null)
                return NotFound();

            _context.Divisoes.Remove(divisao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DivisaoExists(int id)
        {
            return _context.Divisoes.Any(e => e.Id == id);
        }
    }
}
