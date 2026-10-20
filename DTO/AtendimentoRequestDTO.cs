namespace AplicacaoAulaBackend.DTO
{
    public class AtendimentoRequestDTO
    {
        public ulong IdPessoa { get; set; }

        public DateTime DataAtendimento { get; set; }

        public string StatusAtendimento { get; set; }

    }
}
