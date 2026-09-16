using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly InfinityPartDbContext _context;

    public PedidoRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        _context.SaveChanges();
    }

    public void Atualizar(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var pedido = _context.Pedidos.Find(id);

        if (pedido != null)
        {
            _context.Pedidos.Remove(pedido);
            _context.SaveChanges();
        }
    }

    public Pedido? ObterPorId(int id)
    {
        return _context.Pedidos.Find(id);
    }

    public List<Pedido> ObterPorClienteId(int clienteId)
    {
        return _context.Pedidos
            .Where(p => p.ClienteId == clienteId)
            .ToList();
    }

    public List<Pedido> ObterTodos()
    {
        return _context.Pedidos.ToList();
    }
}