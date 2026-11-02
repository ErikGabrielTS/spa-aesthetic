namespace AplicacaoAulaBackend.DTO
{
    public class ItemResponseDTO
    {
        public ulong IdItem { get; set; }
        public string NomeItem { get; set; } = null!;
        public string? DescricaoItem { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public string TipoItem { get; set; } = null!;
        public CategoriaResponseDTO Categoria { get; set; } = null!;

        // Campos exclusivos de Produto
        public decimal? PrecoVenda { get; set; }
        public uint? CodigoPreco { get; set; }
        public string? Unidade { get; set; }
        public long? QuantidadeEstoque { get; set; }
    }
}
