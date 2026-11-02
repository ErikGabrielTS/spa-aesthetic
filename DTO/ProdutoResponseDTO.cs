namespace AplicacaoAulaBackend.DTO
{
    public class ProdutoResponseDTO
    {
        public ulong IdItem { get; set; }

        public string NomeItem { get; set; } = string.Empty;

        public string? DescricaoItem { get; set; }

        public decimal Preco { get; set; }

        public decimal PrecoVenda { get; set; }

        public uint? CodigoPreco { get; set; }

        public string Unidade { get; set; } = string.Empty;

        public bool Ativo { get; set; }

        public long QuantidadeEstoque { get; set; }

        public CategoriaResponseDTO Categoria { get; set; } = new();
    }
}
