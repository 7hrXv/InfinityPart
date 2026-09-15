namespace InfinityPart.UI.Models;

public class ProdutoViewModel
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string CodigoPeca { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public int QuantidadeEstoque { get; set; }

    public int CategoriaId { get; set; }

    public int FabricanteId { get; set; }
}