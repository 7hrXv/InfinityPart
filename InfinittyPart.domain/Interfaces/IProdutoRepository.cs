using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IProdutoRepository
{
    void Adicionar(Produto produto);

    void Atualizar(Produto produto);

    void Remover(int id);

    Produto? ObterPorId(int id);

    List<Produto> ObterTodos();

    List<Produto> ObterPorMarcaId(int marcaId);

    List<Produto> ObterPorNome(string nome);
}