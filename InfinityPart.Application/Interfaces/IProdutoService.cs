using InfinityPart.Application.DTOs.Produtos;

namespace InfinityPart.Application.Interfaces;

public interface IProdutoService
{
    Task<ProdutoDto> CriarAsync(CriarProdutoDto dto);

    IEnumerable<ProdutoDto> Listar();

    ProdutoDto? BuscarPorId(int id);

    IEnumerable<ProdutoDto> BuscarPorMarcaId(int marcaId);

    IEnumerable<ProdutoDto> BuscarPorNome(string nome);

    ProdutoDto? Atualizar(AtualizarProdutoDto dto);

    bool Remover(int id);
}