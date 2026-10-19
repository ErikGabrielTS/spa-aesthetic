namespace AplicacaoAulaBackend.DTO
{
    public class EntradaProdutoRequestDTO
    {
        public ulong IdItem { get; set; }

        public DateTime DataEntradaProduto { get; set; }

        public decimal QuantidadeEntrada { get; set; }

        public string Unidade { get; set; } = null!;

        public decimal ValorUnitarioEntrada { get; set; }
    }
}
