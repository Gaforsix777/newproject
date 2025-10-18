using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetIdentity.Controllers
{
    public class JuegosController : Controller
    {
        [Authorize(Policy = "menoresEdad")]
        [Authorize(Policy = "SoloFemenino")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Usuario")]
        [Authorize(Policy = "SoloFemenino")]
        public IActionResult JuegoEducativo()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "SoloMasculino")]
        public IActionResult Aventuras()
        {
            return View();
        }
    }
}
