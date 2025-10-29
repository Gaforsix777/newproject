using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;

namespace practicamvc.Controllers
{
    [Authorize(Policy = "SoloProveedores")]
    public class ProductoController : Controller
    {
        private readonly ArtesaniasDBContext _context;

        public ProductoController(ArtesaniasDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var productoModel = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (productoModel == null) return NotFound();
            return View(productoModel);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel productoModel)
        {
            TryFixPrecioFromForm(nameof(productoModel.Precio), ref productoModel);

            if (ModelState.IsValid)
            {
                productoModel.Precio = Math.Round(productoModel.Precio, 2, MidpointRounding.AwayFromZero);
                _context.Add(productoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productoModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var productoModel = await _context.Productos.FindAsync(id);
            if (productoModel == null) return NotFound();
            return View(productoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel productoModel)
        {
            if (id != productoModel.Id) return NotFound();

            TryFixPrecioFromForm(nameof(productoModel.Precio), ref productoModel);

            if (ModelState.IsValid)
            {
                try
                {
                    productoModel.Precio = Math.Round(productoModel.Precio, 2, MidpointRounding.AwayFromZero);
                    _context.Update(productoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoModelExists(productoModel.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productoModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var productoModel = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (productoModel == null) return NotFound();
            return View(productoModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoModel = await _context.Productos.FindAsync(id);
            if (productoModel != null) _context.Productos.Remove(productoModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoModelExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }

        private void TryFixPrecioFromForm(string fieldName, ref ProductoModel model)
        {
            if (ModelState.TryGetValue(fieldName, out var entry) && entry.Errors.Count > 0)
            {
                var raw = Request.Form[fieldName].ToString();
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    raw = raw.Trim().Replace(" ", "");
                    raw = raw.Replace(".", "").Replace(",", ".");

                    if (decimal.TryParse(raw, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var parsed))
                    {
                        entry.Errors.Clear();
                        model.Precio = parsed;
                        ModelState.SetModelValue(fieldName, parsed, parsed.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
            }
        }
    }
}
