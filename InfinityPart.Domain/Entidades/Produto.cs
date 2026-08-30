namespace InfinityPart.Entidades;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoPeca { get; set; } = string.Empty; // SKU / Código do Fabricante
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }

    // Relacionamentos
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public int FabricanteId { get; set; }
    public Fabricante? Fabricante { get; set; }
}