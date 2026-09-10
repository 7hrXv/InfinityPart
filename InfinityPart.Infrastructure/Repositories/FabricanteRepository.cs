using InfinittyPart.Domain.Entidades;
using InfinityPart.Domain.Interfaces;
using InfinityPart.Entidades;

namespace InfinityPart.Infrastructure.Repositories;

public class FabricanteRepository : IFabricanteRepository
{
    private readonly InfinityPartDbContext _context;

    public FabricanteRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Fabricante fabricante)
    {
        _context.Fabricantes.Add(fabricante);
        _context.SaveChanges();
    }

    public void Atualizar(Fabricante fabricante)
    {
        _context.Fabricantes.Update(fabricante);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var fabricante = _context.Fabricantes.Find(id);

        if (fabricante == null)
            return;

        _context.Fabricantes.Remove(fabricante);
        _context.SaveChanges();
    }

    public Fabricante? ObterPorId(int id)
    {
        return _context.Fabricantes
            .FirstOrDefault(f => f.Id == id);
    }

    public List<Fabricante> ObterTodos()
    {
        return _context.Fabricantes.ToList();
    }
}