using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin,empleado")]  // Solo admin y empleados pueden acceder
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Orders.ToList());
    }
}
