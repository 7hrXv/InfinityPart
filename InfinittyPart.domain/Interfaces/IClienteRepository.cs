using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IClienteRepository
{
    void Adicionar(Cliente cliente);

    void Atualizar(Cliente cliente);

    void Remover(int id);

    Cliente? ObterPorId(int id);

    Cliente? ObterPorCpf(string cpf);

    Cliente? ObterPorLogin(string identificador);

    List<Cliente> ObterTodos();
}