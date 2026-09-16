using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IMarcaRepository
{
    void Adicionar(Marca marca);

    void Atualizar(Marca marca);

    void Remover(int id);

    Marca? ObterPorId(int id);

    Marca? ObterPorNome(string nome);

    List<Marca> ObterTodos();
}