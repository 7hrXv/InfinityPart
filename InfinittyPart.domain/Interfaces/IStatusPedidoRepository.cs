using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IStatusPedidoRepository
{
    void Adicionar(StatusPedido statusPedido);

    void Atualizar(StatusPedido statusPedido);

    void Remover(int id);

    StatusPedido? ObterPorId(int id);

    StatusPedido? ObterPorNome(string nome);

    List<StatusPedido> ObterTodos();
}