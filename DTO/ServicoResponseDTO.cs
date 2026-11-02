namespace AplicacaoAulaBackend.DTO
{
    public class ServicoResponseDTO
    {
        public ulong IdItem { get; set; }

        public string NomeItem { get; set; } = string.Empty;

        public string? DescricaoItem { get; set; }

        public decimal Preco { get; set; }

        public bool Ativo { get; set; }

        public CategoriaResponseDTO Categoria { get; set; } = new();
    }
}
