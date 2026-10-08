using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class VwProdutosCategoriasEstoque
{
    public ulong IdItem { get; set; }

    public string Item { get; set; } = null!;

    public string? DescricaoItem { get; set; }

    public string AtivoProdutoServico { get; set; } = null!;

    public ulong IdCategoria { get; set; }

    public string Categoria { get; set; } = null!;

    public uint? CodigoPreco { get; set; }

    public decimal PrecoVenda { get; set; }

    public string Unidade { get; set; } = null!;

    public long QuantidadeEstoque { get; set; }
}
