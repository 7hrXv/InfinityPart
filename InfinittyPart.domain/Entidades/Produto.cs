namespace InfinittyPart.Domain.Entidades;

public class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Codigo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public int QuantidadeEstoque { get; set; }

    // Relacionamento com Marca
    public int MarcaId { get; set; }

    public Marca? Marca { get; set; }

    // Relacionamento com os itens dos pedidos
    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}