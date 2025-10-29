using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;

namespace practicamvc.Controllers
{
    public class ProductoModelsController : Controller
    {
        private readonly ArtesaniasDBContext _context;
        public ProductoModelsController(ArtesaniasDBContext context) => _context = context;

        public async Task<IActionResult> Index(string? buscar, string? orden)
        {
            ViewData["OrdenNombre"] = string.IsNullOrEmpty(orden) ? "nombre_desc" : "";
            ViewData["OrdenPrecio"] = orden == "precio_asc" ? "precio_desc" : "precio_asc";
            ViewData["FiltroActual"] = buscar;

            var q = _context.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
                q = q.Where(p => p.Nombre.Contains(buscar) || (p.Descripcion ?? "").Contains(buscar));

            q = orden switch
            {
                "nombre_desc" => q.OrderByDescending(p => p.Nombre),
                "precio_asc" => q.OrderBy(p => p.Precio),
                "precio_desc" => q.OrderByDescending(p => p.Precio),
                _ => q.OrderBy(p => p.Nombre)
            };

            return View(await q.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.Productos.FindAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Productos.AnyAsync(e => e.Id == model.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Productos.FindAsync(id);
            if (model != null) _context.Productos.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
