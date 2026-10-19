namespace AplicacaoAulaBackend.DTO
{
    public class EntradaProdutoResponseDTO
    {
        public ulong IdEntradaProduto { get; set; }

        public ulong IdItem { get; set; }

        public string? Item1 { get; set; }

        public DateTime DataEntradaProduto { get; set; }

        public decimal QuantidadeEntrada { get; set; }

        public string Unidade { get; set; } = null!;

        public decimal ValorUnitarioEntrada { get; set; }
    }
}
