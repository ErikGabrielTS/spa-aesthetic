namespace AplicacaoAulaBackend.DTO
{
    public class EnderecoRequestDTO
    {
        public ulong IdPessoa { get; set; }

        public string Cep { get; set; }

        public string? Numero { get; set; }

        public string? Complemento { get; set; }
    }
}
