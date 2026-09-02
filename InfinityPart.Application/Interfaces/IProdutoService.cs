using InfinityPart.Application.DTOs.Produtos;

namespace InfinityPart.Application.Interfaces;

public interface IProdutoService
{
    Task<ProdutoDto> CriarAsync(CriarProdutoDto dto);

    IEnumerable<ProdutoDto> Listar();

    ProdutoDto? BuscarPorId(int id);

    ProdutoDto? Atualizar(AtualizarProdutoDto dto);

    bool Remover(int id);
}