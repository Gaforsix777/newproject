using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practicamvc.Models;

namespace practicamvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            HomeModel Mode = new HomeModel();
            Mode.Mensaje = "Bienvenido a Artesanías";
            return View(Mode);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Policy = "SoloClientes")]
        public IActionResult PanelCliente()
        {
            return View();
        }

        [Authorize(Policy = "SoloProveedores")]
        public IActionResult PanelProveedor()
        {
            return View();
        }

        [Authorize(Policy = "ClientesYProveedores")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        public IActionResult MiPerfil()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
