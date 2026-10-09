using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Item
{
    public ulong IdItem { get; set; }

    public ulong IdCategoria { get; set; }

    public string Item1 { get; set; } = null!;

    public string? DescricaoItem { get; set; }

    public decimal Preco { get; set; }

    public string TipoItem { get; set; } = null!;

    public string AtivoProdutoServico { get; set; } = null!;

    public virtual ICollection<AtendimentoItem> AtendimentoItems { get; set; } = new List<AtendimentoItem>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Produto? Produto { get; set; }
}
