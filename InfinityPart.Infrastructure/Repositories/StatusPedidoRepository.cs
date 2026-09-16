using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class StatusPedidoRepository : IStatusPedidoRepository
{
    private readonly InfinityPartDbContext _context;

    public StatusPedidoRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(StatusPedido statusPedido)
    {
        _context.StatusPedidos.Add(statusPedido);
        _context.SaveChanges();
    }

    public void Atualizar(StatusPedido statusPedido)
    {
        _context.StatusPedidos.Update(statusPedido);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var status = _context.StatusPedidos.Find(id);

        if (status != null)
        {
            _context.StatusPedidos.Remove(status);
            _context.SaveChanges();
        }
    }

    public StatusPedido? ObterPorId(int id)
    {
        return _context.StatusPedidos.Find(id);
    }

    public StatusPedido? ObterPorNome(string nome)
    {
        return _context.StatusPedidos
            .FirstOrDefault(s => s.Nome == nome);
    }

    public List<StatusPedido> ObterTodos()
    {
        return _context.StatusPedidos.ToList();
    }
}