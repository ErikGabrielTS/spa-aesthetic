namespace AplicacaoAulaBackend.DTO
{
    public class PedidoClienteResponseDTO
    {
        public ulong IdAtendimento { get; set; }

        public DateTime DataAtendimento { get; set; }

        public string StatusAtendimento { get; set; } = null!;

        public List<PedidoClienteItemResponseDTO> Itens { get; set; } = new();
    }
}
