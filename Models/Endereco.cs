using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Endereco
{
    public ulong IdEndereco { get; set; }

    public ulong IdPessoa { get; set; }

    public string Cep { get; set; } = null!;

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;
}
