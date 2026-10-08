using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class EntradaProduto
{
    public ulong IdEntradaProduto { get; set; }

    public ulong IdItem { get; set; }

    public DateTime DataEntradaProduto { get; set; }

    public decimal QuantidadeEntrada { get; set; }

    public string Unidade { get; set; } = null!;

    public decimal ValorUnitarioEntrada { get; set; }

    public virtual Estoque IdItemNavigation { get; set; } = null!;
}
