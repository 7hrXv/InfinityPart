namespace InfinityPart.Application.DTOs.Pedidos;

public class AtualizarPedidoDto
{
    public int Id { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;

    public string ApplicationUserId { get; set; } = string.Empty;
}