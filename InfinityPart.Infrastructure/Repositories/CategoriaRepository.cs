using InfinityPart.Domain.Interfaces;
using InfinittyPart.Domain.Entidades;

namespace InfinityPart.Infrastructure.Repositories
{
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

            if (categoria == null)
                return;

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
        }

        public Categoria ObterPorId(int id)
        {
            return _context.Categorias
                .FirstOrDefault(c => c.Id == id);
        }

        public List<Categoria> ObterTodas()
        {
            return _context.Categorias.ToList();
        }
    }
}