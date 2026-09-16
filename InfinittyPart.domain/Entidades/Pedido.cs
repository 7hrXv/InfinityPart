namespace InfinittyPart.Domain.Entidades;

public class Pedido
{
    public int Id { get; set; }

    public DateTime DataPedido { get; set; } = DateTime.UtcNow;

    public decimal ValorTotal { get; set; }

    // Cliente que realizou o pedido
    public int ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    // Status atual do pedido
    public int StatusPedidoId { get; set; }

    public StatusPedido? StatusPedido { get; set; }

    // Itens que fazem parte do pedido
    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}