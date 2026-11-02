using System.Linq.Expressions;
using AplicacaoAulaBackend.DTO;
using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoAulaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private const string TipoProduto = "P";
        private const string TipoServico = "S";
        private const string AtivoSim = "S";
        private const string AtivoNao = "N";

        private readonly DbAulaBackendContext _context;

        public ItemController(DbAulaBackendContext context)
        {
            _context = context;
        }

        [HttpGet("Produtos")]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> GetProdutos()
        {
            return await _context.Produtos
                .Where(p => p.IdItemNavigation.TipoItem == TipoProduto)
                .Select(ProdutoProjection())
                .ToListAsync();
        }

        [HttpGet("Produto/{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> GetProduto(ulong id)
        {
            var produto = await _context.Produtos
                .Where(p => p.IdItem == id && p.IdItemNavigation.TipoItem == TipoProduto)
                .Select(ProdutoProjection())
                .FirstOrDefaultAsync();

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpPost("Produto")]
        public async Task<ActionResult<ProdutoResponseDTO>> PostProduto(ProdutoRequestDTO dto)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == dto.IdCategoria);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            var nomeItem = dto.NomeItem.Trim();

            if (await _context.Items.AnyAsync(i => i.Item1 == nomeItem))
            {
                return Conflict("Já existe um item cadastrado com este nome.");
            }

            var item = new Item
            {
                IdCategoria = dto.IdCategoria,
                Item1 = nomeItem,
                DescricaoItem = string.IsNullOrWhiteSpace(dto.DescricaoItem) ? null : dto.DescricaoItem.Trim(),
                Preco = dto.Preco,
                TipoItem = TipoProduto,
                AtivoProdutoServico = dto.Ativo ? AtivoSim : AtivoNao,
                Produto = new Produto
                {
                    CodigoPreco = dto.CodigoPreco,
                    PrecoVenda = dto.PrecoVenda,
                    Unidade = dto.Unidade.Trim(),
                }
            };

            _context.Items.Add(item);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Falha ao cadastrar o produto. Verifique os dados informados.");
            }

            var estoque = await _context.Estoques
                .FirstOrDefaultAsync(e => e.IdItem == item.IdItem);

            var response = new ProdutoResponseDTO
            {
                IdItem = item.IdItem,
                NomeItem = item.Item1,
                DescricaoItem = item.DescricaoItem,
                Preco = item.Preco,
                PrecoVenda = item.Produto!.PrecoVenda,
                CodigoPreco = item.Produto!.CodigoPreco,
                Unidade = item.Produto!.Unidade,
                Ativo = item.AtivoProdutoServico == AtivoSim,
                QuantidadeEstoque = estoque?.QuantidadeEstoque ?? 0,
                Categoria = new CategoriaResponseDTO
                {
                    IdCategoria = item.IdCategoria,
                    Descricao = categoria.Descricao,
                }
            };

            return CreatedAtAction(nameof(GetProduto), new { id = item.IdItem }, response);
        }

        [HttpPut("Produto/{id}")]
        public async Task<IActionResult> PutProduto(ulong id, ProdutoRequestDTO dto)
        {
            var produto = await _context.Produtos
                .Include(p => p.IdItemNavigation)
                .FirstOrDefaultAsync(p => p.IdItem == id && p.IdItemNavigation.TipoItem == TipoProduto);

            if (produto == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == dto.IdCategoria);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            var nomeItem = dto.NomeItem.Trim();

            if (await _context.Items.AnyAsync(i => i.Item1 == nomeItem && i.IdItem != id))
            {
                return Conflict("Já existe um item cadastrado com este nome.");
            }

            var item = produto.IdItemNavigation;
            item.IdCategoria = dto.IdCategoria;
            item.Item1 = nomeItem;
            item.DescricaoItem = string.IsNullOrWhiteSpace(dto.DescricaoItem) ? null : dto.DescricaoItem.Trim();
            item.Preco = dto.Preco;
            item.AtivoProdutoServico = dto.Ativo ? AtivoSim : AtivoNao;

            produto.CodigoPreco = dto.CodigoPreco;
            produto.PrecoVenda = dto.PrecoVenda;
            produto.Unidade = dto.Unidade.Trim();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExiste(id))
                {
                    return NotFound();
                }

                throw;
            }
            catch (DbUpdateException)
            {
                return BadRequest("Falha ao atualizar o produto. Verifique os dados informados.");
            }

            return NoContent();
        }

        [HttpDelete("Produto/{id}")]
        public async Task<IActionResult> DeleteProduto(ulong id)
        {
            var produto = await _context.Produtos
                .Include(p => p.IdItemNavigation)
                .Include(p => p.Estoque)
                .ThenInclude(e => e!.EntradaProdutos)
                .FirstOrDefaultAsync(p => p.IdItem == id && p.IdItemNavigation.TipoItem == TipoProduto);

            if (produto == null)
            {
                return NotFound();
            }

            if (await _context.AtendimentoItems.AnyAsync(ai => ai.IdItem == id))
            {
                return Conflict("Não é possível excluir o produto, pois ele está vinculado a atendimentos.");
            }

            if (produto.Estoque != null)
            {
                _context.EntradaProdutos.RemoveRange(produto.Estoque.EntradaProdutos);
                _context.Estoques.Remove(produto.Estoque);
            }

            _context.Produtos.Remove(produto);
            _context.Items.Remove(produto.IdItemNavigation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Não foi possível excluir o produto.");
            }

            return NoContent();
        }

        [HttpGet("Servicos")]
        public async Task<ActionResult<IEnumerable<ServicoResponseDTO>>> GetServicos()
        {
            return await _context.Items
                .Where(i => i.TipoItem == TipoServico)
                .Select(ServicoProjection())
                .ToListAsync();
        }

        [HttpGet("Servico/{id}")]
        public async Task<ActionResult<ServicoResponseDTO>> GetServico(ulong id)
        {
            var servico = await _context.Items
                .Where(i => i.IdItem == id && i.TipoItem == TipoServico)
                .Select(ServicoProjection())
                .FirstOrDefaultAsync();

            if (servico == null)
            {
                return NotFound();
            }

            return servico;
        }

        [HttpPost("Servico")]
        public async Task<ActionResult<ServicoResponseDTO>> PostServico(ServicoRequestDTO dto)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == dto.IdCategoria);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            var nomeItem = dto.NomeItem.Trim();

            if (await _context.Items.AnyAsync(i => i.Item1 == nomeItem))
            {
                return Conflict("Já existe um item cadastrado com este nome.");
            }

            var item = new Item
            {
                IdCategoria = dto.IdCategoria,
                Item1 = nomeItem,
                DescricaoItem = string.IsNullOrWhiteSpace(dto.DescricaoItem) ? null : dto.DescricaoItem.Trim(),
                Preco = dto.Preco,
                TipoItem = TipoServico,
                AtivoProdutoServico = dto.Ativo ? AtivoSim : AtivoNao,
            };

            _context.Items.Add(item);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Falha ao cadastrar o serviço. Verifique os dados informados.");
            }

            var response = new ServicoResponseDTO
            {
                IdItem = item.IdItem,
                NomeItem = item.Item1,
                DescricaoItem = item.DescricaoItem,
                Preco = item.Preco,
                Ativo = item.AtivoProdutoServico == AtivoSim,
                Categoria = new CategoriaResponseDTO
                {
                    IdCategoria = item.IdCategoria,
                    Descricao = categoria.Descricao,
                }
            };

            return CreatedAtAction(nameof(GetServico), new { id = item.IdItem }, response);
        }

        [HttpPut("Servico/{id}")]
        public async Task<IActionResult> PutServico(ulong id, ServicoRequestDTO dto)
        {
            var servico = await _context.Items
                .FirstOrDefaultAsync(i => i.IdItem == id && i.TipoItem == TipoServico);

            if (servico == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == dto.IdCategoria);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            var nomeItem = dto.NomeItem.Trim();

            if (await _context.Items.AnyAsync(i => i.Item1 == nomeItem && i.IdItem != id))
            {
                return Conflict("Já existe um item cadastrado com este nome.");
            }

            servico.IdCategoria = dto.IdCategoria;
            servico.Item1 = nomeItem;
            servico.DescricaoItem = string.IsNullOrWhiteSpace(dto.DescricaoItem) ? null : dto.DescricaoItem.Trim();
            servico.Preco = dto.Preco;
            servico.AtivoProdutoServico = dto.Ativo ? AtivoSim : AtivoNao;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicoExiste(id))
                {
                    return NotFound();
                }

                throw;
            }
            catch (DbUpdateException)
            {
                return BadRequest("Falha ao atualizar o serviço. Verifique os dados informados.");
            }

            return NoContent();
        }

        [HttpDelete("Servico/{id}")]
        public async Task<IActionResult> DeleteServico(ulong id)
        {
            var servico = await _context.Items
                .FirstOrDefaultAsync(i => i.IdItem == id && i.TipoItem == TipoServico);

            if (servico == null)
            {
                return NotFound();
            }

            if (await _context.AtendimentoItems.AnyAsync(ai => ai.IdItem == id))
            {
                return Conflict("Não é possível excluir o serviço, pois ele está vinculado a atendimentos.");
            }

            _context.Items.Remove(servico);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Não foi possível excluir o serviço.");
            }

            return NoContent();
        }

        private bool ProdutoExiste(ulong id)
        {
            return _context.Produtos
                .Any(p => p.IdItem == id);
        }

        private bool ServicoExiste(ulong id)
        {
            return _context.Items
                .Any(i => i.IdItem == id && i.TipoItem == TipoServico);
        }

        private static Expression<Func<Produto, ProdutoResponseDTO>> ProdutoProjection()
        {
            return p => new ProdutoResponseDTO
            {
                IdItem = p.IdItem,
                NomeItem = p.IdItemNavigation.Item1,
                DescricaoItem = p.IdItemNavigation.DescricaoItem,
                Preco = p.IdItemNavigation.Preco,
                PrecoVenda = p.PrecoVenda,
                CodigoPreco = p.CodigoPreco,
                Unidade = p.Unidade,
                Ativo = p.IdItemNavigation.AtivoProdutoServico == AtivoSim,
                QuantidadeEstoque = p.Estoque == null ? 0L : (long)p.Estoque.QuantidadeEstoque,
                Categoria = new CategoriaResponseDTO
                {
                    IdCategoria = p.IdItemNavigation.IdCategoria,
                    Descricao = p.IdItemNavigation.IdCategoriaNavigation.Descricao,
                }
            };
        }

        private static Expression<Func<Item, ServicoResponseDTO>> ServicoProjection()
        {
            return i => new ServicoResponseDTO
            {
                IdItem = i.IdItem,
                NomeItem = i.Item1,
                DescricaoItem = i.DescricaoItem,
                Preco = i.Preco,
                Ativo = i.AtivoProdutoServico == AtivoSim,
                Categoria = new CategoriaResponseDTO
                {
                    IdCategoria = i.IdCategoria,
                    Descricao = i.IdCategoriaNavigation.Descricao
                }
            };
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ItemResponseDTO>>> GetItens()
        {
            var itens = await _context.Items
                .Include(i => i.IdCategoriaNavigation)
                .Include(i => i.Produto)
                    .ThenInclude(p => p!.Estoque)
                .OrderBy(i => i.Item1)
                .Select(i => new ItemResponseDTO
                {
                    IdItem = i.IdItem,
                    NomeItem = i.Item1,
                    DescricaoItem = i.DescricaoItem,
                    Preco = i.Preco,
                    Ativo = i.AtivoProdutoServico == AtivoSim,
                    TipoItem = i.TipoItem,
                    Categoria = new CategoriaResponseDTO
                    {
                        IdCategoria = i.IdCategoria,
                        Descricao = i.IdCategoriaNavigation.Descricao
                    },
                    PrecoVenda = i.Produto != null ? i.Produto.PrecoVenda : null,
                    CodigoPreco = i.Produto != null ? i.Produto.CodigoPreco : null,
                    Unidade = i.Produto != null ? i.Produto.Unidade : null,
                    QuantidadeEstoque = i.Produto != null && i.Produto.Estoque != null
                        ? (long)i.Produto.Estoque.QuantidadeEstoque
                        : null
                })
                .ToListAsync();

            return itens;
        }
    };
};