using AplicacaoAulaBackend.DTO;
using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoAulaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtendimentoController : ControllerBase
    {
        private readonly DbAulaBackendContext _context;

        public AtendimentoController(DbAulaBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtendimentoResponseDTO>>> GetAtendimentos()
        {
            return await _context.Atendimentos
                .Select(a => new AtendimentoResponseDTO
                {
                    IdAtendimento = a.IdAtendimento,
                    IdPessoa = a.IdPessoa,
                    DataAtendimento = a.DataAtendimento,
                    StatusAtendimento = a.StatusAtendimento,
                    Itens = a.AtendimentoItems.Select(i => new AtendimentoItemResponseDTO
                    {
                        IdItem = i.IdItem,
                        IdFuncionario = i.IdFuncionario,
                        QuantidadeItens = i.QuantidadeItens,
                        ValorUnitarioEntrada = i.ValorUnitarioEntrada
                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("Pessoa/{id}")]
        public async Task<ActionResult<IEnumerable<AtendimentoResponseDTO>>> GetAtendimentosByIdPessoa(ulong id)
        {
            return await _context.Atendimentos
                .Where(a => a.IdPessoa == id)
                .Select(a => new AtendimentoResponseDTO
                {
                    IdAtendimento = a.IdAtendimento,
                    IdPessoa = a.IdPessoa,
                    DataAtendimento = a.DataAtendimento,
                    StatusAtendimento = a.StatusAtendimento,
                    Itens = a.AtendimentoItems.Select(i => new AtendimentoItemResponseDTO
                    {
                        IdItem = i.IdItem,
                        IdFuncionario = i.IdFuncionario,
                        QuantidadeItens = i.QuantidadeItens,
                        ValorUnitarioEntrada = i.ValorUnitarioEntrada
                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AtendimentoResponseDTO>> GetAtendimentoById(ulong id)
        {
            var atendimento = await _context.Atendimentos
                .Where(a => a.IdAtendimento == id)
                .Select(a => new AtendimentoResponseDTO
                {
                    IdAtendimento = a.IdAtendimento,
                    IdPessoa = a.IdPessoa,
                    DataAtendimento = a.DataAtendimento,
                    StatusAtendimento = a.StatusAtendimento,
                    Itens = a.AtendimentoItems.Select(i => new AtendimentoItemResponseDTO
                    {
                        IdItem = i.IdItem,
                        IdFuncionario = i.IdFuncionario,
                        QuantidadeItens = i.QuantidadeItens,
                        ValorUnitarioEntrada = i.ValorUnitarioEntrada
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (atendimento == null)
            {
                return NotFound();
            }

            return Ok(atendimento);
        }

        [HttpPost("{id}/Item")]
        public async Task<ActionResult<AtendimentoItemResponseDTO>> CreateAtendimentoItem(ulong id, AtendimentoItemRequestDTO dto)
        {
            var atendimento = await _context.Atendimentos
                .Include(a => a.AtendimentoItems)
                .FirstOrDefaultAsync(a => a.IdAtendimento == id);

            if (atendimento == null)
            {
                return NotFound("Atendimento não encontrado.");
            }

            if (!await _context.Items.AnyAsync(i => i.IdItem == dto.IdItem))
            {
                return NotFound($"Item {dto.IdItem} não encontrado.");
            }

            if (!await _context.Pessoas.AnyAsync(p => p.IdPessoa == dto.IdFuncionario))
            {
                return NotFound($"Funcionario {dto.IdFuncionario} não encontrado.");
            }

            if (atendimento.AtendimentoItems.Any(i => i.IdItem == dto.IdItem))
            {
                return Conflict("Item já vinculado ao atendimento.");
            }

            var valorUnitario = await ObterValorUnitario(dto);

            if (valorUnitario == null)
            {
                return BadRequest($"Não foi possível determinar o valor do item {dto.IdItem}.");
            }

            var item = new AtendimentoItem
            {
                IdAtendimento = id,
                IdItem = dto.IdItem,
                IdFuncionario = dto.IdFuncionario,
                QuantidadeItens = dto.QuantidadeItens,
                ValorUnitarioEntrada = valorUnitario.Value
            };

            _context.AtendimentoItems.Add(item);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Falha ao adicionar item. Verifique o estoque e os dados do item.");
            }

            var response = new AtendimentoItemResponseDTO
            {
                IdItem = item.IdItem,
                IdFuncionario = item.IdFuncionario,
                QuantidadeItens = item.QuantidadeItens,
                ValorUnitarioEntrada = item.ValorUnitarioEntrada
            };

            return CreatedAtAction(nameof(GetAtendimentoById), new { id = item.IdAtendimento }, response);
        }

        [HttpDelete("{id}/Item/{idItem}")]
        public async Task<IActionResult> DeleteAtendimentoItem(ulong id, ulong idItem)
        {
            var item = await _context.AtendimentoItems
                .FirstOrDefaultAsync(i => i.IdAtendimento == id && i.IdItem == idItem);

            if (item == null)
            {
                return NotFound();
            }

            _context.AtendimentoItems.Remove(item);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AtendimentoResponseDTO>> CreateAtendimento(AtendimentoRequestDTO dto)
        {
            if (!await _context.Pessoas.AnyAsync(p => p.IdPessoa == dto.IdPessoa))
            {
                return NotFound("Pessoa não encontrada.");
            }

            var atendimento = new Atendimento
            {
                IdPessoa = dto.IdPessoa,
                DataAtendimento = dto.DataAtendimento == default ? DateTime.Now : dto.DataAtendimento,
                StatusAtendimento = dto.StatusAtendimento
            };

            _context.Atendimentos.Add(atendimento);

            await _context.SaveChangesAsync();

            var response = new AtendimentoResponseDTO
            {
                IdAtendimento = atendimento.IdAtendimento,
                IdPessoa = atendimento.IdPessoa,
                DataAtendimento = atendimento.DataAtendimento,
                StatusAtendimento = atendimento.StatusAtendimento,
                Itens = atendimento.AtendimentoItems.Select(i => new AtendimentoItemResponseDTO
                {
                    IdItem = i.IdItem,
                    IdFuncionario = i.IdFuncionario,
                    QuantidadeItens = i.QuantidadeItens,
                    ValorUnitarioEntrada = i.ValorUnitarioEntrada
                }).ToList()
            };

            return CreatedAtAction(nameof(GetAtendimentoById), new { id = atendimento.IdAtendimento }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAtendimento(ulong id, AtendimentoRequestDTO dto)
        {
            var atendimento = await _context.Atendimentos
                .FirstOrDefaultAsync(a => a.IdAtendimento == id);

            if (atendimento == null)
            {
                return NotFound();
            }

            if (atendimento.IdPessoa != dto.IdPessoa)
            {
                return Conflict("Não é possível alterar a pessoa do atendimento.");
            }

            atendimento.DataAtendimento = dto.DataAtendimento;
            atendimento.StatusAtendimento = dto.StatusAtendimento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtendimentoExiste(id))
                {
                    return NotFound();
                }

                throw;
            }
            catch (DbUpdateException)
            {
                return BadRequest("Falha ao atualizar atendimento.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtendimento(ulong id)
        {
            var atendimento = await _context.Atendimentos
                .Include(a => a.AtendimentoItems)
                .FirstOrDefaultAsync(a => a.IdAtendimento == id);

            if (atendimento == null)
            {
                return NotFound();
            }

            _context.AtendimentoItems.RemoveRange(atendimento.AtendimentoItems);
            _context.Atendimentos.Remove(atendimento);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<decimal?> ObterValorUnitario(AtendimentoItemRequestDTO itemDto)
        {
            if (itemDto.ValorUnitarioEntrada.HasValue)
            {
                return itemDto.ValorUnitarioEntrada.Value;
            }

            var precoVenda = await _context.Produtos
                .Where(p => p.IdItem == itemDto.IdItem)
                .Select(p => (decimal?)p.PrecoVenda)
                .FirstOrDefaultAsync();

            if (precoVenda.HasValue)
            {
                return precoVenda.Value;
            }

            return await _context.Items
                .Where(i => i.IdItem == itemDto.IdItem)
                .Select(i => (decimal?)i.Preco)
                .FirstOrDefaultAsync();
        }

        private bool AtendimentoExiste(ulong id)
        {
            return _context.Atendimentos
                .Any(a => a.IdAtendimento == id);
        }
    }
}
