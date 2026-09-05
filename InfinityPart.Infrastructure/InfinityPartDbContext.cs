using InfinityPart.Domain.Entidades;
using InfinityPart.Entidades;
using InfinittyPart.Domain.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfinityPart.Infrastructure
{
    public class InfinityPartDbContext : IdentityDbContext<ApplicationUser>
    {
        public InfinityPartDbContext(DbContextOptions<InfinityPartDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ValorTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Pecas)
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Fabricante)
                .WithMany(f => f.Pecas)
                .HasForeignKey(p => p.FabricanteId);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}