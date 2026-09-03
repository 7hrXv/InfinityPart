using System.Collections.Generic;
using InfinityPart.Domain.Entidades;

namespace InfinityPart.Domain.Interfaces
{
    // Interface para gerenciar os Clientes da InfinityPart
    public interface IClienteRepository
    {
        // Cadastra um novo cliente
        void Adicionar(Cliente cliente);

        // Atualiza os dados de um cliente (ex: endereço, telefone)
        void Atualizar(Cliente cliente);

        // Remove a conta de um cliente
        void Remover(int id);

        // Busca cliente pelo código ID
        Cliente ObterPorId(int id);

        // Busca cliente pelo CPF (muito útil na hora do login/checkout)
        Cliente ObterPorCpf(string cpf);

        // Lista todos os clientes cadastrados
        List<Cliente> ObterTodos();
    }
}