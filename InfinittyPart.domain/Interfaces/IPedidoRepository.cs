using InfinityPart.Domain.Entidades;
using InfinityPart.Entidades;
using System.Collections.Generic;

namespace InfinityPart.Domain.Interfaces
{
    // Interface para gerenciar os Pedidos de compra
    public interface IPedidoRepository
    {
        // Salva um novo pedido realizado no site
        void Criar(Pedido pedido);

        // Atualiza as informações ou status do pedido
        void Atualizar(Pedido pedido);

        // Busca um pedido pelo número ID do pedido
        Pedido ObterPorId(int id);

        // Lista todos os pedidos feitos por um cliente específico
        List<Pedido> ObterPorClienteId(int clienteId);

        // Lista todos os pedidos cadastrados no sistema (para a tela do Administrador)
        List<Pedido> ObterTodos();
    }
}