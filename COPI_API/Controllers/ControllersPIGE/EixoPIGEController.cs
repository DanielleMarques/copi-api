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
    public class EixoPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EixoPIGEController(AppDbContext context) => _context = context;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EixoPIGEOutputDTO>>> Get()
        {
            var eixos = await _context.Set<EixoPIGE>().ToListAsync();
            return Ok(eixos.Select(e => new EixoPIGEOutputDTO
            {
                Id = e.Id,
                Nome = e.Nome ?? string.Empty,
                Peso = e.Peso
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EixoPIGE>> GetById(int id)
        {
            var eixo = await _context.Set<EixoPIGE>().FindAsync(id);
            if (eixo == null)
                return NotFound();
            return Ok(eixo);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<ActionResult<EixoPIGE>> Post(EixoPIGE dto)
        {
            _context.Set<EixoPIGE>().Add(dto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> Put(int id, EixoPIGE dto)
        {
            if (id != dto.Id)
                return BadRequest();
            _context.Entry(dto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Set<EixoPIGE>().AnyAsync(e => e.Id == id))
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
            var eixo = await _context.Set<EixoPIGE>().FindAsync(id);
            if (eixo == null)
                return NotFound();
            _context.Set<EixoPIGE>().Remove(eixo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
