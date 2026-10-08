using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class VwAtendimentosDetalhe
{
    public ulong IdAtendimento { get; set; }

    public DateTime DataAtendimento { get; set; }

    public string StatusAtendimento { get; set; } = null!;

    public ulong IdCliente { get; set; }

    public string Cliente { get; set; } = null!;

    public string CpfCliente { get; set; } = null!;

    public ulong IdItem { get; set; }

    public string Produto { get; set; } = null!;

    public uint QuantidadeItens { get; set; }

    public decimal ValorUnitarioEntrada { get; set; }

    public decimal ValorTotalItem { get; set; }

    public ulong IdFuncionario { get; set; }

    public string Funcionario { get; set; } = null!;
}
