namespace InfinittyPart.Domain.Entidades;

public class ItemPedido
{
    public int Id { get; set; }

    // Pedido
    public int PedidoId { get; set; }

    public Pedido? Pedido { get; set; }

    // Produto
    public int ProdutoId { get; set; }

    public Produto? Produto { get; set; }

    // Quantidade comprada
    public int Quantidade { get; set; }

    // Preço do produto no momento da compra
    public decimal PrecoUnitario { get; set; }

    // Total deste item
    public decimal Subtotal => Quantidade * PrecoUnitario;
}