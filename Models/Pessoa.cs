using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Pessoa
{
    public ulong IdPessoa { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public DateOnly DataNascimento { get; set; }

    public string Sexo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string AtivoPessoa { get; set; } = null!;

    public virtual ICollection<AtendimentoItem> AtendimentoItems { get; set; } = new List<AtendimentoItem>();

    public virtual ICollection<Atendimento> Atendimentos { get; set; } = new List<Atendimento>();

    public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

    public virtual Funcionario? Funcionario { get; set; }
}
