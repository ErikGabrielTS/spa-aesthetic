using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly DbAulaBackendContext _context;

    public CategoriaController(DbAulaBackendContext context)
    {
        _context = context;
    }

    // GET: api/Categoria
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        return await _context.Categoria.ToListAsync();
    }


    // GET: api/Categoria/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoria(ulong id)
    {
        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(c => c.IdCategoria == id);

        if (categoria == null)
        {
            return NotFound();
        }

        return categoria;
    }


    // POST: api/Categoria
    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
    {
        _context.Categoria.Add(categoria);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategoria),
            new { id = categoria.IdCategoria },
            categoria
        );
    }


    // PUT: api/Categoria/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoria(
        ulong id,
        Categoria categoria)
    {
        if (id != categoria.IdCategoria)
        {
            return BadRequest();
        }

        _context.Entry(categoria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoriaExiste(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }


    // DELETE: api/Categoria/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(ulong id)
    {
        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(c => c.IdCategoria == id);

        if (categoria == null)
        {
            return NotFound();
        }

        _context.Categoria.Remove(categoria);

        await _context.SaveChangesAsync();

        return NoContent();
    }


    private bool CategoriaExiste(ulong id)
    {
        return _context.Categoria
            .Any(e => e.IdCategoria == id);
    }
}