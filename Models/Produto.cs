using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Produto
{
    public ulong IdItem { get; set; }

    public uint? CodigoPreco { get; set; }

    public decimal PrecoVenda { get; set; }

    public string Unidade { get; set; } = null!;

    public virtual Estoque? Estoque { get; set; }

    public virtual Item IdItemNavigation { get; set; } = null!;
}
