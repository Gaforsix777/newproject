using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin,empleado")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db) => _db = db;

        // Listado
        public async Task<IActionResult> Index()
        {
            var list = await _db.Orders
                .Include(o => o.Cliente)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();
            return View(list);
        }

        // Detalle
        public async Task<IActionResult> Details(int id)
        {
            var o = await _db.Orders
                .Include(x => x.Cliente)
                .Include(x => x.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (o == null) return NotFound();
            return View(o);
        }

        // Crear (GET)
        public async Task<IActionResult> Create()
        {
            ViewBag.Clientes = await _db.Users
                .Where(u => u.Rol == UserRole.cliente)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
            ViewBag.Productos = await _db.Products
                .OrderBy(p => p.Nombre)
                .ToListAsync();
            return View(new OrderCreateVM());
        }

        // Crear (POST)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateVM vm)
        {
            // Repoblar combos si hay errores
            ViewBag.Clientes = await _db.Users.Where(u => u.Rol == UserRole.cliente).OrderBy(u => u.Nombre).ToListAsync();
            ViewBag.Productos = await _db.Products.OrderBy(p => p.Nombre).ToListAsync();

            if (!ModelState.IsValid) return View(vm);

            // Normaliza items válidos
            var rows = vm.Items
                .Where(it => it.ProductId.HasValue && it.Cantidad.HasValue && it.Cantidad.Value > 0)
                .ToList();
            if (!rows.Any())
            {
                ModelState.AddModelError(string.Empty, "Agrega al menos un producto.");
                return View(vm);
            }

            // Cargar productos y validar stock
            var pids = rows.Select(r => r.ProductId!.Value).Distinct().ToList();
            var prods = await _db.Products.Where(p => pids.Contains(p.Id)).ToDictionaryAsync(p => p.Id);

            foreach (var r in rows)
            {
                var p = prods[r.ProductId!.Value];
                if (r.Cantidad!.Value > p.Stock)
                {
                    ModelState.AddModelError(string.Empty, $"Stock insuficiente para {p.Nombre}. Disponible: {p.Stock}");
                    return View(vm);
                }
            }

            // Crear pedido
            var order = new Order
            {
                ClienteId = vm.ClienteId,
                Fecha = DateTime.UtcNow,
                Estado = OrderStatus.Pendiente,
                Items = new List<OrderItem>()
            };

            decimal total = 0m;
            foreach (var r in rows)
            {
                var p = prods[r.ProductId!.Value];
                var subtotal = p.Precio * r.Cantidad!.Value;
                order.Items.Add(new OrderItem
                {
                    ProductId = p.Id,
                    Cantidad = r.Cantidad!.Value,
                    Subtotal = subtotal
                });
                total += subtotal;

                // descontar stock
                p.Stock -= r.Cantidad!.Value;
            }
            order.Total = total;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            TempData["msg"] = "Pedido creado correctamente.";
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        // Cambiar estado (POST)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, OrderStatus estado)
        {
            var o = await _db.Orders.FindAsync(id);
            if (o == null) return NotFound();

            o.Estado = estado;
            await _db.SaveChangesAsync();
            TempData["msg"] = "Estado actualizado.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
