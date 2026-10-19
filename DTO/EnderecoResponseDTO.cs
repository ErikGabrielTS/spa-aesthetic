namespace AplicacaoAulaBackend.DTO
{
    public class EnderecoResponseDTO
    {
        public ulong IdEndereco { get; set; }

        public ulong IdPessoa { get; set; }

        public string Cep { get; set; }

        public string? Numero { get; set; }

        public string? Complemento { get; set; }
    }
}
