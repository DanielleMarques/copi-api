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
    public class NivelPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;
        public NivelPIGEController(AppDbContext context) => _context = context;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NivelPIGEOutputDTO>>> Get()
        {
            var niveis = await _context.NiveisPIGE.ToListAsync();
            return Ok(niveis.Select(n => new NivelPIGEOutputDTO
            {
                Id = n.Id,
                Nome = n.Nome ?? string.Empty,
                Valor = n.Valor
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<NivelPIGE>> GetById(int id)
        {
            var nivel = await _context.NiveisPIGE.FindAsync(id);
            if (nivel == null)
                return NotFound();
            return Ok(nivel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<ActionResult<NivelPIGE>> Post(NivelPIGE nivel)
        {
            _context.NiveisPIGE.Add(nivel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = nivel.Id }, nivel);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> Put(int id, NivelPIGE nivel)
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
                if (!await NivelExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> Delete(int id)
        {
            var nivel = await _context.NiveisPIGE.FindAsync(id);
            if (nivel == null)
                return NotFound();

            _context.NiveisPIGE.Remove(nivel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> NivelExists(int id)
        {
            return await _context.NiveisPIGE.AnyAsync(n => n.Id == id);
        }
    }
}
