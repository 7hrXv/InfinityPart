using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Entidades;

public class Fabricante
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? CNPJ { get; set; }

    public ICollection<Produto> Pecas { get; set; } = new List<Produto>();
}