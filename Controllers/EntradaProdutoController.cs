using AplicacaoAulaBackend.DTO;
using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoAulaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaProdutoController : ControllerBase
    {
        private readonly DbAulaBackendContext _context;

        public EntradaProdutoController(DbAulaBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntradaProdutoResponseDTO>>> GetEntradaProdutos()
        {
            return await _context.EntradaProdutos
                .Select(ep => new EntradaProdutoResponseDTO
                {
                    IdEntradaProduto = ep.IdEntradaProduto,
                    IdItem = ep.IdItem,
                    Item1 = ep.IdItemNavigation.IdItemNavigation.IdItemNavigation.Item1,
                    DataEntradaProduto = ep.DataEntradaProduto,
                    QuantidadeEntrada = ep.QuantidadeEntrada,
                    Unidade = ep.Unidade,
                    ValorUnitarioEntrada = ep.ValorUnitarioEntrada

                })
                .ToListAsync();
        }

        [HttpGet("Item/{id}")]
        public async Task<ActionResult<IEnumerable<EntradaProdutoResponseDTO>>> GetEntradaProdutosByIdItem(ulong id)
        {
            return await _context.EntradaProdutos
                .Where(ep => ep.IdItem == id)
                .Select(ep => new EntradaProdutoResponseDTO
                {
                    IdEntradaProduto = ep.IdEntradaProduto,
                    IdItem = ep.IdItem,
                    Item1 = ep.IdItemNavigation.IdItemNavigation.IdItemNavigation.Item1,
                    DataEntradaProduto = ep.DataEntradaProduto,
                    QuantidadeEntrada = ep.QuantidadeEntrada,
                    Unidade = ep.Unidade,
                    ValorUnitarioEntrada = ep.ValorUnitarioEntrada

                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntradaProdutoResponseDTO>> GetEntradaProduto(ulong id)
        {
            var entradaProduto = await _context.EntradaProdutos
                .Where(ep => ep.IdEntradaProduto == id)
                .Select(ep => new EntradaProdutoResponseDTO
                {
                    IdEntradaProduto = ep.IdEntradaProduto,
                    IdItem = ep.IdItem,
                    Item1 = ep.IdItemNavigation.IdItemNavigation.IdItemNavigation.Item1,
                    DataEntradaProduto = ep.DataEntradaProduto,
                    QuantidadeEntrada = ep.QuantidadeEntrada,
                    Unidade = ep.Unidade,
                    ValorUnitarioEntrada = ep.ValorUnitarioEntrada

                })
                .FirstOrDefaultAsync();

            if (entradaProduto == null) {
                return NotFound();
            }

            return entradaProduto;
        }

        [HttpPost]
        public async Task<ActionResult<EntradaProdutoResponseDTO>> createEntradaProduto(EntradaProdutoRequestDTO dto)
        {
            var entradaProduto = new EntradaProduto
            {
                IdItem = dto.IdItem,
                DataEntradaProduto = dto.DataEntradaProduto,
                QuantidadeEntrada = dto.QuantidadeEntrada,
                Unidade = dto.Unidade,
                ValorUnitarioEntrada = dto.ValorUnitarioEntrada
            };

            _context.EntradaProdutos.Add(entradaProduto);

            await _context.SaveChangesAsync();

            var response = new EntradaProdutoResponseDTO
            {
                IdEntradaProduto = entradaProduto.IdEntradaProduto,
                IdItem = entradaProduto.IdItem,
                DataEntradaProduto = entradaProduto.DataEntradaProduto,
                QuantidadeEntrada = entradaProduto.QuantidadeEntrada,
                Unidade = entradaProduto.Unidade,
                ValorUnitarioEntrada = entradaProduto.ValorUnitarioEntrada
            };

            return CreatedAtAction(
                nameof(GetEntradaProduto),
                new { id = entradaProduto.IdEntradaProduto },
                response
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateEntradaProduto(
        ulong id,
        EntradaProdutoRequestDTO dto)
        {
            var entradaProduto = await _context.EntradaProdutos
                .FirstOrDefaultAsync(c => c.IdEntradaProduto == id);

            if (entradaProduto == null)
            {
                return NotFound();
            }

            entradaProduto.IdItem = dto.IdItem;
            entradaProduto.DataEntradaProduto = dto.DataEntradaProduto;
            entradaProduto.QuantidadeEntrada = dto.QuantidadeEntrada;
            entradaProduto.Unidade = dto.Unidade;
            entradaProduto.ValorUnitarioEntrada = dto.ValorUnitarioEntrada;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntradaProdutoExiste(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntradaProduto(ulong id)
        {
            var entradaProduto = await _context.EntradaProdutos
                .FirstOrDefaultAsync(ep => ep.IdEntradaProduto == id);

            if (entradaProduto == null)
            {
                return NotFound();
            }

            _context.EntradaProdutos.Remove(entradaProduto);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntradaProdutoExiste(ulong id)
        {
            return _context.EntradaProdutos
                .Any(ep => ep.IdEntradaProduto == id);
        }
    }
}
