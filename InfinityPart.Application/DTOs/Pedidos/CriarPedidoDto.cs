namespace InfinityPart.Application.DTOs.Pedidos;

public class CriarPedidoDto
{
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Pendente";

    public string ApplicationUserId { get; set; } = string.Empty;
}