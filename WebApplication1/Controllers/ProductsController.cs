using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Authorize] // cualquiera logueado
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index(string? q, string? categoria, decimal? min, decimal? max)
        {
            bool isStaff = User.IsInRole("admin") || User.IsInRole("empleado");

            var query = _db.Products.AsQueryable();

            if (!isStaff)
                query = query.Where(p => p.Stock > 0); // clientes: solo disponibles

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p => p.Nombre.Contains(q) || (p.Descripcion != null && p.Descripcion.Contains(q)));

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria == categoria);

            if (min.HasValue) query = query.Where(p => p.Precio >= min.Value);
            if (max.HasValue) query = query.Where(p => p.Precio <= max.Value);

            var list = await query.OrderBy(p => p.Nombre).ToListAsync();
            ViewBag.IsStaff = isStaff; // para la vista
            return View(list);
        }

        [Authorize(Roles = "admin,empleado")]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,empleado")]
        public async Task<IActionResult> Create(Product p)
        {
            if (!ModelState.IsValid) return View(p);
            _db.Add(p);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin,empleado")]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _db.Products.FindAsync(id);
            return p == null ? NotFound() : View(p);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,empleado")]
        public async Task<IActionResult> Edit(int id, Product p)
        {
            if (id != p.Id) return NotFound();
            if (!ModelState.IsValid) return View(p);
            _db.Update(p);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin,empleado")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.FindAsync(id);
            return p == null ? NotFound() : View(p);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,empleado")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p != null) _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
