using COPI_API.Models;
using COPI_API.Models.PIBPEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COPI_API.Controllers.ControllersPIBP
{
    [ApiController]
    [Route("api/[controller]")]
    public class NiveisController : ControllerBase
    {
        private readonly AppDbContext _context;
        public NiveisController(AppDbContext context) => _context = context;

        // Obter todos os níveis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nivel>>> Get()
        {
            var niveis = await _context.Niveis.ToListAsync();
            return Ok(niveis);
        }

        // Obter um nível por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Nivel>> GetById(int id)
        {
            var nivel = await _context.Niveis.FindAsync(id);
            if (nivel == null)
                return NotFound();
            return Ok(nivel);
        }

        // Criar um novo nível
        [HttpPost]
        public async Task<ActionResult<Nivel>> Post(Nivel nivel)
        {
            _context.Niveis.Add(nivel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = nivel.Id }, nivel);
        }

        // Atualizar um nível existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Nivel nivel)
        {
            if (id != nivel.Id)
                return BadRequest();

            _context.Entry(nivel).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NivelExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Deletar um nível
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nivel = await _context.Niveis.FindAsync(id);
            if (nivel == null)
                return NotFound();

            _context.Niveis.Remove(nivel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Verificar se o nível existe
        private bool NivelExists(int id) => _context.Niveis.Any(n => n.Id == id);
    }
}
