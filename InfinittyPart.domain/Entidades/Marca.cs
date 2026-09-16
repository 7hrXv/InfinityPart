namespace InfinittyPart.Domain.Entidades;

public class Marca
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Cnpj { get; set; }

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}