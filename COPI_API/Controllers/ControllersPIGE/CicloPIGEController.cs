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
    public class CicloPIGEController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CicloPIGEController(AppDbContext context) => _context = context;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CicloPIGEOutputDTO>>> Get()
        {
            var ciclos = await _context.CiclosPIGE.ToListAsync();
            return Ok(ciclos.Select(c => new CicloPIGEOutputDTO
            {
                Id = c.Id,
                Nome = c.Nome ?? string.Empty,
                Encerrado = c.Encerrado,
                DataInicio = c.DataInicio,
                DataFim = c.DataFim
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CicloPIGE>> GetById(int id)
        {
            var ciclo = await _context.CiclosPIGE.FindAsync(id);
            if (ciclo == null)
                return NotFound();
            return Ok(ciclo);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<ActionResult<CicloPIGE>> Post(CicloPIGE dto)
        {
            _context.CiclosPIGE.Add(dto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Gestor,GestorPIBP,UsuarioPIBP")]
        public async Task<IActionResult> Put(int id, CicloPIGE dto)
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
                if (!await _context.CiclosPIGE.AnyAsync(c => c.Id == id))
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
            var ciclo = await _context.CiclosPIGE.FindAsync(id);
            if (ciclo == null)
                return NotFound();
            _context.CiclosPIGE.Remove(ciclo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
