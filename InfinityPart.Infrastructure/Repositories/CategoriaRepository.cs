using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly InfinityPartDbContext _context;

    public CategoriaRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        _context.SaveChanges();
    }

    public void Atualizar(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var categoria = _context.Categorias.Find(id);

        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
        }
    }

    public Categoria? ObterPorId(int id)
    {
        return _context.Categorias.Find(id);
    }

    public IEnumerable<Categoria> ObterTodos()
    {
        return _context.Categorias.ToList();
    }
}