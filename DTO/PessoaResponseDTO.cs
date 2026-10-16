namespace AplicacaoAulaBackend.DTO
{
    public class PessoaResponseDTO
    {
        public ulong IdPessoa { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public DateOnly DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }
    }
}
