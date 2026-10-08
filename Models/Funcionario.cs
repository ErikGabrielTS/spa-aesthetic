using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Funcionario
{
    public ulong IdPessoa { get; set; }

    public string Cargo { get; set; } = null!;

    public DateOnly DataAdmissao { get; set; }

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;
}
