using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin,empleado")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string? q, string? categoria, decimal? min, decimal? max)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(p => p.Nombre.Contains(q) || (p.Descripcion != null && p.Descripcion.Contains(q)));
            if (!string.IsNullOrWhiteSpace(categoria)) query = query.Where(p => p.Categoria == categoria);
            if (min.HasValue) query = query.Where(p => p.Precio >= min);
            if (max.HasValue) query = query.Where(p => p.Precio <= max);

            var products = await query.OrderBy(p => p.Nombre).ToListAsync();
            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create() => View(new Product());

        // POST: Products/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["msg"] = "Producto creado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();
            if (!ModelState.IsValid) return View(product);

            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["msg"] = "Producto actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["msg"] = "Producto eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
