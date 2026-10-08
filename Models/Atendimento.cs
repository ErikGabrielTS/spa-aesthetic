using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Atendimento
{
    public ulong IdAtendimento { get; set; }

    public ulong IdPessoa { get; set; }

    public DateTime DataAtendimento { get; set; }

    public string StatusAtendimento { get; set; } = null!;

    public virtual ICollection<AtendimentoItem> AtendimentoItems { get; set; } = new List<AtendimentoItem>();

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;
}
