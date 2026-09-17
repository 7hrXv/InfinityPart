namespace InfinityPart.Desktop.Models
{
    /// <summary>
    /// Espelha InfinityPart.Application.DTOs.Produtos.ProdutoDto (contrato JSON da API).
    /// </summary>
    public class ProdutoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int MarcaId { get; set; }
    }

    public class CriarProdutoModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int MarcaId { get; set; }
    }
}
