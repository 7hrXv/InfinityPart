using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class MarcaRepository : IMarcaRepository
{
    private readonly InfinityPartDbContext _context;

    public MarcaRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Marca marca)
    {
        _context.Marcas.Add(marca);
        _context.SaveChanges();
    }

    public void Atualizar(Marca marca)
    {
        _context.Marcas.Update(marca);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var marca = _context.Marcas.Find(id);

        if (marca != null)
        {
            _context.Marcas.Remove(marca);
            _context.SaveChanges();
        }
    }

    public Marca? ObterPorId(int id)
    {
        return _context.Marcas.Find(id);
    }

    public Marca? ObterPorNome(string nome)
    {
        return _context.Marcas
            .FirstOrDefault(m => m.Nome == nome);
    }

    public List<Marca> ObterTodos()
    {
        return _context.Marcas.ToList();
    }
}