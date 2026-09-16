namespace InfinittyPart.Domain.Entidades;

public class StatusPedido
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}