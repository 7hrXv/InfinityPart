using InfinittyPart.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace InfinityPart.Infrastructure;

public class InfinityPartDbContext : DbContext
{
    public InfinityPartDbContext(DbContextOptions<InfinityPartDbContext> options)
        : base(options)
    {
    }

    public DbSet<Administrador> Administradores { get; set; }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Pedido> Pedidos { get; set; }

    public DbSet<ItemPedido> ItensPedido { get; set; }

    public DbSet<Produto> Produtos { get; set; }

    public DbSet<Marca> Marcas { get; set; }

    public DbSet<StatusPedido> StatusPedidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // ADMINISTRADOR
        // =========================

        modelBuilder.Entity<Administrador>()
            .Property(a => a.Nome)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Administrador>()
            .Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Administrador>()
            .Property(a => a.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        modelBuilder.Entity<Administrador>()
            .Property(a => a.Telefone)
            .HasMaxLength(20);

        // Hash da senha (PBKDF2). Nunca armazenar senha em texto puro.
        modelBuilder.Entity<Administrador>()
            .Property(a => a.SenhaHash)
            .IsRequired()
            .HasMaxLength(500)
            .HasDefaultValue(string.Empty);

        modelBuilder.Entity<Administrador>()
            .Property(a => a.Ativo)
            .IsRequired()
            .HasDefaultValue(true);

        modelBuilder.Entity<Administrador>()
            .Ignore(a => a.PossuiSenhaDefinida);

        modelBuilder.Entity<Administrador>()
            .HasIndex(a => a.Email);


        // =========================
        // CLIENTE
        // =========================

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Telefone)
            .HasMaxLength(20);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Cep)
            .HasMaxLength(10);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Endereco)
            .HasMaxLength(200);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Numero)
            .HasMaxLength(20);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Cidade)
            .HasMaxLength(100);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Estado)
            .HasMaxLength(2);

        modelBuilder.Entity<Cliente>()
           .Property(c => c.SenhaHash)
           .IsRequired()
           .HasMaxLength(500);


        // =========================
        // MARCA
        // =========================

        modelBuilder.Entity<Marca>()
            .Property(m => m.Nome)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Marca>()
            .Property(m => m.Cnpj)
            .HasMaxLength(18);


        // =========================
        // PRODUTO
        // =========================

        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Produto>()
            .Property(p => p.Codigo)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Produto>()
            .Property(p => p.Descricao)
            .HasMaxLength(500);

        modelBuilder.Entity<Produto>()
            .Property(p => p.Preco)
            .HasPrecision(18, 2);


        // Produto -> Marca
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Marca)
            .WithMany(m => m.Produtos)
            .HasForeignKey(p => p.MarcaId)
            .OnDelete(DeleteBehavior.Restrict);


        // =========================
        // STATUS DO PEDIDO
        // =========================

        modelBuilder.Entity<StatusPedido>()
            .Property(s => s.Nome)
            .IsRequired()
            .HasMaxLength(50);


        // =========================
        // PEDIDO
        // =========================

        modelBuilder.Entity<Pedido>()
            .Property(p => p.ValorTotal)
            .HasPrecision(18, 2);


        // Cliente -> Pedidos
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);


        // StatusPedido -> Pedidos
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.StatusPedido)
            .WithMany(s => s.Pedidos)
            .HasForeignKey(p => p.StatusPedidoId)
            .OnDelete(DeleteBehavior.Restrict);


        // =========================
        // ITEM DO PEDIDO
        // =========================

        modelBuilder.Entity<ItemPedido>()
            .Property(i => i.PrecoUnitario)
            .HasPrecision(18, 2);


        // Pedido -> ItensPedido
        modelBuilder.Entity<ItemPedido>()
            .HasOne(i => i.Pedido)
            .WithMany(p => p.Itens)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);


        // Produto -> ItensPedido
        modelBuilder.Entity<ItemPedido>()
            .HasOne(i => i.Produto)
            .WithMany(p => p.ItensPedido)
            .HasForeignKey(i => i.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}