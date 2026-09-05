using InfinityPart.Domain.Entidades;
using InfinityPart.Entidades;
using System.Collections.Generic;

namespace InfinityPart.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        void Criar(Pedido pedido);

        void Atualizar(Pedido pedido);

        Pedido ObterPorId(int id);

        List<Pedido> ObterPorClienteId(string clienteId);

        List<Pedido> ObterTodos();
    }
}