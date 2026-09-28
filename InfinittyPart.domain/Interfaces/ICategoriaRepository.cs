using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface ICategoriaRepository
{
    IEnumerable<Categoria> ObterTodos();

    Categoria? ObterPorId(int id);

    void Adicionar(Categoria categoria);

    void Atualizar(Categoria categoria);

    void Remover(int id);
}