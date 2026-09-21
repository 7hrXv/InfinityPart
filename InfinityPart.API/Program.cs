using InfinittyPart.Domain.Interfaces;
using InfinityPart.Application.Interfaces;
using InfinityPart.Application.Services;
using InfinityPart.Infrastructure;
using InfinityPart.Infrastructure.Repositories;
using InfinityPart.Infrastructure.Seguranca;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// CORS
// =====================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEnd", policy =>
    {
        policy
            .WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =====================================================
// CONTROLLERS
// =====================================================

builder.Services.AddControllers();

// =====================================================
// BANCO DE DADOS
// =====================================================

builder.Services.AddDbContext<InfinityPartDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// =====================================================
// SERVICES - APPLICATION
// =====================================================

builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IItemPedidoService, ItemPedidoService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IStatusPedidoService, StatusPedidoService>();

// =====================================================
// SEGURANÇA
// =====================================================

builder.Services.AddSingleton<ISenhaHasher, SenhaHasher>();

// =====================================================
// REPOSITORIES - INFRASTRUCTURE
// =====================================================

builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IStatusPedidoRepository, StatusPedidoRepository>();

// =====================================================
// SWAGGER
// =====================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =====================================================
// BUILD DA APLICAÇÃO
// =====================================================

var app = builder.Build();

// =====================================================
// CORS
// =====================================================

app.UseCors("FrontEnd");

// =====================================================
// SWAGGER
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// =====================================================
// HTTPS
// =====================================================

app.UseHttpsRedirection();

// =====================================================
// AUTORIZAÇÃO
// =====================================================

app.UseAuthorization();

// =====================================================
// CONTROLLERS
// =====================================================

app.MapControllers();

// =====================================================
// EXECUÇÃO
// =====================================================

app.Run();