using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using practicamvc.Models;
using practicamvc.Models;

namespace practicamvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        HomeModel Mode = new HomeModel();
        Mode.Mensaje = "Hola Mundito!";
        //Mode.Destinatario = "Docentes";
        return View(Mode);
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}