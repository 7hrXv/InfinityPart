namespace InfinityPart.Application.DTOs.Pedidos;

public class PedidoDto
{
    public int Id { get; set; }

    public DateTime DataPedido { get; set; }

    public decimal ValorTotal { get; set; }

    public int ClienteId { get; set; }

    public int StatusPedidoId { get; set; }
}