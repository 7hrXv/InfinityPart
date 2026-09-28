namespace InfinittyPart.Domain.Entidades;

public class Categoria
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    // Produtos pertencentes à categoria
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}