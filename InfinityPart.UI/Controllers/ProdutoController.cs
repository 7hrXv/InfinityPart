using InfinityPart.UI.Models;
using InfinityPart.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.UI.Controllers;

public class ProdutoController : Controller
{
    private readonly ApiService _apiService;

    public ProdutoController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await _apiService
            .GetAsync<List<ProdutoViewModel>>("api/Produto");

        return View(produtos ?? new List<ProdutoViewModel>());
    }
}