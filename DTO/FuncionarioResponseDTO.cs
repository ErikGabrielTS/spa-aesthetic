using AplicacaoAulaBackend.Models;

namespace AplicacaoAulaBackend.DTO
{
    public class FuncionarioResponseDTO
    {
        public ulong IdFuncionario{ get; set; }

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

