namespace InfinityPart.Application.DTOs.Pedidos;

public class AtualizarPedidoDto
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int StatusPedidoId { get; set; }

    public decimal ValorTotal { get; set; }
}