using System.ComponentModel.DataAnnotations;

namespace AplicacaoAulaBackend.DTO
{
    public class ProdutoRequestDTO
    {
        [Required(ErrorMessage = "Informe o nome do produto.")]
        [StringLength(40, ErrorMessage = "O nome do produto deve ter no máximo 40 caracteres.")]
        public string NomeItem { get; set; } = string.Empty;

        public ulong IdCategoria { get; set; }

        [StringLength(200, ErrorMessage = "A descrição do produto deve ter no máximo 200 caracteres.")]
        public string? DescricaoItem { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço do produto deve ser maior ou igual a zero.")]
        public decimal Preco { get; set; }

        public bool Ativo { get; set; } = true;

        public uint? CodigoPreco { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior ou igual a zero.")]
        public decimal PrecoVenda { get; set; }

        [Required(ErrorMessage = "Informe a unidade do produto.")]
        [StringLength(10, ErrorMessage = "A unidade deve ter no máximo 10 caracteres.")]
        public string Unidade { get; set; } = string.Empty;
    }
}
