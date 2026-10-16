namespace AplicacaoAulaBackend.DTO
{
    public class FuncionarioRequestDTO
    {
        public string Nome { get; set; }

        public string Cpf { get; set; }

        public DateOnly DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string Cargo { get; set; } = null!;

        public DateOnly DataAdmissao { get; set; }
    }
}

