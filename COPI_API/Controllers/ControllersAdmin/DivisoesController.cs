using COPI_API.Models.AdminEntities;
using COPI_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
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
        //[Authorize(Roles = "Admin,Gestor")]
        public async Task<ActionResult<IEnumerable<Divisao>>> GetDivisoes()
        {
            return await _context.Divisoes.ToListAsync();
        }

        // GET: api/Divisoes/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Gestor")]
        public async Task<ActionResult<Divisao>> GetDivisao(int id)
        {
            var divisao = await _context.Divisoes.FindAsync(id);

            if (divisao == null)
                return NotFound();

            return divisao;
        }

        public class DivisaoCreateDto
        {
            [Required]
            public string Nome { get; set; } = string.Empty;
        }

        // POST: api/Divisoes
        [HttpPost]
        [Authorize(Roles = "Admin,Gestor")]
        public async Task<ActionResult<Divisao>> PostDivisao(DivisaoCreateDto dto)
        {
            var divisao = new Divisao
            {
                Nome = dto.Nome
            };

            _context.Divisoes.Add(divisao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDivisao),
                new { id = divisao.Id },
                divisao
            );
        }

        // PUT: api/Divisoes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Gestor")]
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
        [Authorize(Roles = "Admin,Gestor")]
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
