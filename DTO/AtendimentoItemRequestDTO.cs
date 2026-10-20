namespace AplicacaoAulaBackend.DTO
{
    public class AtendimentoItemRequestDTO
    {
        public ulong IdItem { get; set; }

        public ulong IdFuncionario { get; set; }

        public uint QuantidadeItens { get; set; }

        public decimal? ValorUnitarioEntrada { get; set; }
    }
}
