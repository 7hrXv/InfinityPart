using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly InfinityPartDbContext _context;

    public ProdutoRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Produto produto)
    {
        _context.Produtos.Add(produto);
        _context.SaveChanges();
    }

    public void Atualizar(Produto produto)
    {
        _context.Produtos.Update(produto);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var produto = _context.Produtos.Find(id);

        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }
    }

    public Produto? ObterPorId(int id)
    {
        return _context.Produtos.Find(id);
    }

    public List<Produto> ObterTodos()
    {
        return _context.Produtos.ToList();
    }

    public List<Produto> ObterPorMarcaId(int marcaId)
    {
        return _context.Produtos
            .Where(p => p.MarcaId == marcaId)
            .ToList();
    }

    public List<Produto> ObterPorNome(string nome)
    {
        return _context.Produtos
            .Where(p => p.Nome.Contains(nome))
            .ToList();
    }
}