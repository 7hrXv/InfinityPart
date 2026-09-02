using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;
using InfinityPart.Application.DTOs.Produtos;
using InfinityPart.Application.Interfaces;

namespace InfinityPart.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoDto> CriarAsync(CriarProdutoDto dto)
    {
        var produto = new Produto
        {
            Nome = dto.Nome,
            CodigoPeca = dto.CodigoPeca,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            QuantidadeEstoque = dto.QuantidadeEstoque,
            CategoriaId = dto.CategoriaId,
            FabricanteId = dto.FabricanteId
        };

        await _produtoRepository.AdicionarAsync(produto);

        return new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            CodigoPeca = produto.CodigoPeca,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            CategoriaId = produto.CategoriaId,
            FabricanteId = produto.FabricanteId
        };
    }

    public IEnumerable<ProdutoDto> Listar()
    {
        var produtos = _produtoRepository.ObterTodos();

        return produtos.Select(produto => new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            CodigoPeca = produto.CodigoPeca,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            CategoriaId = produto.CategoriaId,
            FabricanteId = produto.FabricanteId
        });
    }

    public ProdutoDto? BuscarPorId(int id)
    {
        var produto = _produtoRepository.ObterPorId(id);

        if (produto == null)
            return null;

        return new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            CodigoPeca = produto.CodigoPeca,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            CategoriaId = produto.CategoriaId,
            FabricanteId = produto.FabricanteId
        };
    }

    public ProdutoDto? Atualizar(AtualizarProdutoDto dto)
    {
        var produto = _produtoRepository.ObterPorId(dto.Id);

        if (produto == null)
            return null;

        produto.Nome = dto.Nome;
        produto.CodigoPeca = dto.CodigoPeca;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.QuantidadeEstoque = dto.QuantidadeEstoque;
        produto.CategoriaId = dto.CategoriaId;
        produto.FabricanteId = dto.FabricanteId;

        _produtoRepository.Atualizar(produto);

        return new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            CodigoPeca = produto.CodigoPeca,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            CategoriaId = produto.CategoriaId,
            FabricanteId = produto.FabricanteId
        };
    }

    public bool Remover(int id)
    {
        var produto = _produtoRepository.ObterPorId(id);

        if (produto == null)
            return false;

        _produtoRepository.Remover(id);

        return true;
    }
}
