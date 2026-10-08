using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class VwClientesEndereco
{
    public ulong IdPessoa { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string AtivoPessoa { get; set; } = null!;

    public ulong? IdEndereco { get; set; }

    public string? Cep { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }
}
