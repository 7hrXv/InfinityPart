using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly InfinityPartDbContext _context;

    public ClienteRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        _context.SaveChanges();
    }

    public void Atualizar(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var cliente = _context.Clientes.Find(id);

        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
        }
    }

    public Cliente? ObterPorId(int id)
    {
        return _context.Clientes.Find(id);
    }

    public Cliente? ObterPorCpf(string cpf)
    {
        return _context.Clientes
            .FirstOrDefault(c => c.Cpf == cpf);
    }

    public Cliente? ObterPorLogin(string identificador)
    {
        return _context.Clientes
            .FirstOrDefault(c =>
                c.Email == identificador ||
                c.Cpf == identificador);
    }

    public List<Cliente> ObterTodos()
    {
        return _context.Clientes.ToList();
    }
}