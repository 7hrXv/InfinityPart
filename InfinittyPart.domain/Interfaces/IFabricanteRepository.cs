using InfinittyPart.Domain.Entidades;
using InfinityPart.Entidades;

namespace InfinityPart.Domain.Interfaces;

public interface IFabricanteRepository
{
    void Adicionar(Fabricante fabricante);

    void Atualizar(Fabricante fabricante);

    void Remover(int id);

    Fabricante? ObterPorId(int id);

    List<Fabricante> ObterTodos();
}