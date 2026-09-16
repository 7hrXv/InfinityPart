using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class ItemPedidoRepository : IItemPedidoRepository
{
    private readonly InfinityPartDbContext _context;

    public ItemPedidoRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(ItemPedido itemPedido)
    {
        _context.ItensPedido.Add(itemPedido);
        _context.SaveChanges();
    }

    public void Atualizar(ItemPedido itemPedido)
    {
        _context.ItensPedido.Update(itemPedido);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var item = _context.ItensPedido.Find(id);

        if (item != null)
        {
            _context.ItensPedido.Remove(item);
            _context.SaveChanges();
        }
    }

    public ItemPedido? ObterPorId(int id)
    {
        return _context.ItensPedido.Find(id);
    }

    public List<ItemPedido> ObterPorPedidoId(int pedidoId)
    {
        return _context.ItensPedido
            .Where(i => i.PedidoId == pedidoId)
            .ToList();
    }

    public List<ItemPedido> ObterPorProdutoId(int produtoId)
    {
        return _context.ItensPedido
            .Where(i => i.ProdutoId == produtoId)
            .ToList();
    }

    public List<ItemPedido> ObterTodos()
    {
        return _context.ItensPedido.ToList();
    }
}