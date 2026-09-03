namespace InfinityPart.Application.DTOs.Pedidos;

public class PedidoDto
{
    public int Id { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;

    public string ApplicationUserId { get; set; } = string.Empty;
}