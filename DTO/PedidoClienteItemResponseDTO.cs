namespace AplicacaoAulaBackend.DTO
{
    public class PedidoClienteItemResponseDTO
    {
        public ulong IdItem { get; set; }

        public string NomeItem { get; set; } = null!;

        public ulong IdFuncionario { get; set; }

        public string Funcionario { get; set; } = null!;

        public uint QuantidadeItens { get; set; }

        public decimal ValorUnitarioEntrada { get; set; }
    }
}
