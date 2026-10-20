namespace AplicacaoAulaBackend.DTO
{
    public class AtendimentoResponseDTO
    {
        public ulong IdAtendimento { get; set; }

        public ulong IdPessoa { get; set; }

        public DateTime DataAtendimento { get; set; }

        public string StatusAtendimento { get; set; }

        public List<AtendimentoItemResponseDTO> Itens { get; set; }
    }
}
