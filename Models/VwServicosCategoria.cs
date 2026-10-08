using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class VwServicosCategoria
{
    public ulong IdItem { get; set; }

    public string Item { get; set; } = null!;

    public string? DescricaoItem { get; set; }

    public decimal Preco { get; set; }

    public string AtivoProdutoServico { get; set; } = null!;

    public ulong IdCategoria { get; set; }

    public string Categoria { get; set; } = null!;
}
