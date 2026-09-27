using Microsoft.AspNetCore.Mvc;

namespace InfinityPart.UI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect("/index.html");
    }

    public IActionResult Privacy()
    {
        return View();
    }
}