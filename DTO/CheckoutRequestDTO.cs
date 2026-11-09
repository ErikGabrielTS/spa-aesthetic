namespace AplicacaoAulaBackend.DTO
{
    public class CheckoutRequestDTO
    {
        public ulong IdPessoa { get; set; }

        public ulong IdFuncionario { get; set; }

        public List<CheckoutItemRequestDTO> Itens { get; set; } = new();
    }
}
