using InfinittyPart.Domain.Entidades;

namespace InfinityPart.Entidades;

public class Fabricante
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? CNPJ { get; set; }

    public ICollection<Produto> Pecas { get; set; } = new List<Produto>();
}