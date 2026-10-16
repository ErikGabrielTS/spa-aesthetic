namespace AplicacaoAulaBackend.DTO
{
    public class EnderecoDTO
    {
        public ulong IdPessoa { get; set; }

        public string Cep { get; set; }

        public string? Numero { get; set; }

        public string? Complemento { get; set; }
    }
}
