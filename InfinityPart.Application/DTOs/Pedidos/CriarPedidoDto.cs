namespace InfinityPart.Application.DTOs.Pedidos;

public class CriarPedidoDto
{
    public int ClienteId { get; set; }

    public int StatusPedidoId { get; set; }

    public decimal ValorTotal { get; set; }
}