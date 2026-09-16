namespace InfinityPart.Application.DTOs.Produtos;

public class ProdutoDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Codigo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public int QuantidadeEstoque { get; set; }

    public int MarcaId { get; set; }
}