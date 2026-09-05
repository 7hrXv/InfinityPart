using InfinityPart.Domain.Interfaces;
using InfinityPart.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InfinityPart.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly InfinityPartDbContext _context;

        public PedidoRepository(InfinityPartDbContext context)
        {
            _context = context;
        }

        public void Criar(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();
        }

        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }

        public Pedido ObterPorId(int id)
        {
            return _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Pedido> ObterPorClienteId(string clienteId)
        {
            return _context.Pedidos
                .Where(p => p.ApplicationUserId == clienteId)
                .Include(p => p.Usuario)
                .ToList();
        }

        public List<Pedido> ObterTodos()
        {
            return _context.Pedidos
                .Include(p => p.Usuario)
                .ToList();
        }
    }
}