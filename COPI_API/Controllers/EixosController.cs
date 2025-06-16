using COPI_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EixosController : ControllerBase
{
    private readonly AppDbContext _context;
    public EixosController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Eixo>>> Get() => await _context.Eixos.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Eixo>> Post(Eixo eixo)
    {
        _context.Eixos.Add(eixo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = eixo.Id }, eixo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Eixo eixo)
    {
        if (id != eixo.Id) return BadRequest();

        _context.Entry(eixo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Eixos.AnyAsync(e => e.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eixo = await _context.Eixos.FindAsync(id);
        if (eixo == null) return NotFound();

        _context.Eixos.Remove(eixo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
