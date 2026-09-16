using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IItemPedidoRepository
{
    void Adicionar(ItemPedido itemPedido);

    void Atualizar(ItemPedido itemPedido);

    void Remover(int id);

    ItemPedido? ObterPorId(int id);

    List<ItemPedido> ObterPorPedidoId(int pedidoId);

    List<ItemPedido> ObterPorProdutoId(int produtoId);

    List<ItemPedido> ObterTodos();
}