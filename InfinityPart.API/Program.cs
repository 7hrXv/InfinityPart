using InfinittyPart.Domain.Interfaces;
using InfinityPart.Application.Interfaces;
using InfinityPart.Application.Services;
using InfinityPart.Domain.Entidades;
using InfinityPart.Domain.Interfaces;
using InfinityPart.Infrastructure;
using InfinityPart.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços da API
builder.Services.AddControllers();

// Banco de dados
builder.Services.AddDbContext<InfinityPartDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Identity
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InfinityPartDbContext>();

// Services da Application
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();

// Repositories da Infrastructure
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();