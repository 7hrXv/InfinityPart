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
            Codigo = dto.Codigo,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            QuantidadeEstoque = dto.QuantidadeEstoque,
            Image = dto.Image,
            MarcaId = dto.MarcaId
        };

        _produtoRepository.Adicionar(produto);

        return MapearParaDto(produto);
    }

    public IEnumerable<ProdutoDto> Listar()
    {
        var produtos = _produtoRepository.ObterTodos();

        return produtos.Select(MapearParaDto);
    }

    public ProdutoDto? BuscarPorId(int id)
    {
        var produto = _produtoRepository.ObterPorId(id);

        if (produto == null)
            return null;

        return MapearParaDto(produto);
    }

    public IEnumerable<ProdutoDto> BuscarPorMarcaId(int marcaId)
    {
        var produtos = _produtoRepository.ObterPorMarcaId(marcaId);

        return produtos.Select(MapearParaDto);
    }

    public IEnumerable<ProdutoDto> BuscarPorNome(string nome)
    {
        var produtos = _produtoRepository.ObterPorNome(nome);

        return produtos.Select(MapearParaDto);
    }

    public ProdutoDto? Atualizar(AtualizarProdutoDto dto)
    {
        var produto = _produtoRepository.ObterPorId(dto.Id);

        if (produto == null)
            return null;

        produto.Nome = dto.Nome;
        produto.Codigo = dto.Codigo;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.QuantidadeEstoque = dto.QuantidadeEstoque;
        produto.Image = dto.Image;
        produto.MarcaId = dto.MarcaId;

        _produtoRepository.Atualizar(produto);

        return MapearParaDto(produto);
    }

    public bool Remover(int id)
    {
        var produto = _produtoRepository.ObterPorId(id);

        if (produto == null)
            return false;

        _produtoRepository.Remover(id);

        return true;
    }

    private static ProdutoDto MapearParaDto(Produto produto)
    {
        return new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Codigo = produto.Codigo,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            Image = produto.Image,
            MarcaId = produto.MarcaId
        };
    }
}