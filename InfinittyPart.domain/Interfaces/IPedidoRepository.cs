using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IPedidoRepository
{
    void Adicionar(Pedido pedido);

    void Atualizar(Pedido pedido);

    void Remover(int id);

    Pedido? ObterPorId(int id);

    List<Pedido> ObterPorClienteId(int clienteId);

    List<Pedido> ObterTodos();
}