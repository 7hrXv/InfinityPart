using InfinityPart.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// MVC
// =====================================================

builder.Services.AddControllersWithViews();

// =====================================================
// CONEXÃO DA UI COM A API
// =====================================================

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]!
    );
});

// =====================================================
// BUILD
// =====================================================

var app = builder.Build();

// =====================================================
// CONFIGURAÇÕES DO AMBIENTE
// =====================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// =====================================================
// ARQUIVOS ESTÁTICOS
// =====================================================

app.UseStaticFiles();

// =====================================================
// ROTEAMENTO
// =====================================================

app.UseRouting();

// =====================================================
// AUTORIZAÇÃO
// =====================================================

app.UseAuthorization();

// =====================================================
// ROTA PADRÃO
// =====================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();