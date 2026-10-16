using AplicacaoAulaBackend.DTO;
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
    public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> GetCategorias()
    {
        return await _context.Categoria
            .Select(c => new CategoriaResponseDTO
            {
                IdCategoria = c.IdCategoria,
                Descricao = c.Descricao
            })
            .ToListAsync();
    }


    // GET: api/Categoria/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaResponseDTO>> GetCategoria(ulong id)
    {
        var categoria = await _context.Categoria
            .Where(c => c.IdCategoria == id)
            .Select(c => new CategoriaResponseDTO
            {
                IdCategoria = c.IdCategoria,
                Descricao = c.Descricao
            })
            .FirstOrDefaultAsync();

        if (categoria == null)
        {
            return NotFound();
        }

        return categoria;
    }


    // POST: api/Categoria
    [HttpPost]
    public async Task<ActionResult<CategoriaResponseDTO>> PostCategoria(CategoriaRequestDTO dto)
    {
        var categoria = new Categoria
        {
            Descricao = dto.Descricao,
        };

        _context.Categoria.Add(categoria);

        await _context.SaveChangesAsync();

        var response = new CategoriaResponseDTO
        {
            IdCategoria = categoria.IdCategoria,
            Descricao = categoria.Descricao
        };

        return CreatedAtAction(
            nameof(GetCategoria),
            new { id = categoria.IdCategoria },
            response
        );
    }


    // PUT: api/Categoria/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoria(
        ulong id,
        CategoriaRequestDTO dto)
    {
        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(c => c.IdCategoria == id);

        if (categoria == null)
        {
            return NotFound();
        }

        categoria.Descricao = dto.Descricao;

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