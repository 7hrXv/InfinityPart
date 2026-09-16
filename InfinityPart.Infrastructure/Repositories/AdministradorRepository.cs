using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class AdministradorRepository : IAdministradorRepository
{
    private readonly InfinityPartDbContext _context;

    public AdministradorRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Administrador administrador)
    {
        _context.Administradores.Add(administrador);
        _context.SaveChanges();
    }

    public void Atualizar(Administrador administrador)
    {
        _context.Administradores.Update(administrador);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var administrador = _context.Administradores.Find(id);

        if (administrador != null)
        {
            _context.Administradores.Remove(administrador);
            _context.SaveChanges();
        }
    }

    public Administrador? ObterPorId(int id)
    {
        return _context.Administradores.Find(id);
    }

    public Administrador? ObterPorCpf(string cpf)
    {
        return _context.Administradores
            .FirstOrDefault(a => a.Cpf == cpf);
    }

    public List<Administrador> ObterTodos()
    {
        return _context.Administradores.ToList();
    }
}