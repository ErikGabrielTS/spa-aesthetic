using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Estoque
{
    public ulong IdItem { get; set; }

    public uint QuantidadeEstoque { get; set; }

    public virtual ICollection<EntradaProduto> EntradaProdutos { get; set; } = new List<EntradaProduto>();

    public virtual Produto IdItemNavigation { get; set; } = null!;
}
