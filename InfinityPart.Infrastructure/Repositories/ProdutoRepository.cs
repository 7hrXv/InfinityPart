using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;
using InfinityPart.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfinityPart.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly InfinityPartDbContext _context;

        public ProdutoRepository(InfinityPartDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto == null)
                return;

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }

        public Produto ObterPorId(int id)
        {
            return _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fabricante)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Produto> ObterTodos()
        {
            return _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fabricante)
                .ToList();
        }

        public List<Produto> ObterPorCategoria(int categoriaId)
        {
            return _context.Produtos
                .Where(p => p.CategoriaId == categoriaId)
                .Include(p => p.Categoria)
                .Include(p => p.Fabricante)
                .ToList();
        }

        public List<Produto> ObterPorNome(string nome)
        {
            return _context.Produtos
                .Where(p => p.Nome.Contains(nome))
                .Include(p => p.Categoria)
                .Include(p => p.Fabricante)
                .ToList();
        }

        public async Task<IEnumerable<object>> ListarTodosAsync()
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fabricante)
                .ToListAsync();
        }

        public async Task BuscarPorIdAsync(int id)
        {
            await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}