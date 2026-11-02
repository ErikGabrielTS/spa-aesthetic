using System.ComponentModel.DataAnnotations;

namespace AplicacaoAulaBackend.DTO
{
    public class ServicoRequestDTO
    {
        [Required(ErrorMessage = "Informe o nome do serviço.")]
        [StringLength(40, ErrorMessage = "O nome do serviço deve ter no máximo 40 caracteres.")]
        public string NomeItem { get; set; } = string.Empty;

        public ulong IdCategoria { get; set; }

        [StringLength(200, ErrorMessage = "A descrição do serviço deve ter no máximo 200 caracteres.")]
        public string? DescricaoItem { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço do serviço deve ser maior ou igual a zero.")]
        public decimal Preco { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
