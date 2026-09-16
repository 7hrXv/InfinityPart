using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IAdministradorRepository
{
    void Adicionar(Administrador administrador);

    void Atualizar(Administrador administrador);

    void Remover(int id);

    Administrador? ObterPorId(int id);

    Administrador? ObterPorCpf(string cpf);

    List<Administrador> ObterTodos();
}