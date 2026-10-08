using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class AtendimentoItem
{
    public ulong IdAtendimento { get; set; }

    public ulong IdItem { get; set; }

    public ulong IdFuncionario { get; set; }

    public uint QuantidadeItens { get; set; }

    public decimal ValorUnitarioEntrada { get; set; }

    public virtual Atendimento IdAtendimentoNavigation { get; set; } = null!;

    public virtual Pessoa IdFuncionarioNavigation { get; set; } = null!;

    public virtual Item IdItemNavigation { get; set; } = null!;
}
