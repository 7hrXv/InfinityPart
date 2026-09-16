namespace InfinityPart.Application.DTOs.ItensPedido;

public class CriarItemPedidoDto
{
    public int PedidoId { get; set; }

    public int ProdutoId { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }
}