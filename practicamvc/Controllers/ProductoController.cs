using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;

namespace practicamvc.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ArtesaniasDBContext _context;

        public ProductoController(ArtesaniasDBContext context)
        {
            _context = context;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var productoModel = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (productoModel == null) return NotFound();

            return View(productoModel);
        }

        // GET: Producto/Create
        public IActionResult Create() => View();

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel productoModel)
        {
            // Fallback: si el binder falló por separador decimal, intentamos parsear manualmente
            TryFixPrecioFromForm(nameof(productoModel.Precio), ref productoModel);

            if (ModelState.IsValid)
            {
                // Seguridad: redondear a 2 decimales
                productoModel.Precio = Math.Round(productoModel.Precio, 2, MidpointRounding.AwayFromZero);

                _context.Add(productoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productoModel);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var productoModel = await _context.Productos.FindAsync(id);
            if (productoModel == null) return NotFound();

            return View(productoModel);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock")] ProductoModel productoModel)
        {
            if (id != productoModel.Id) return NotFound();

            // Fallback de parseo por si el binder no aceptó la coma
            TryFixPrecioFromForm(nameof(productoModel.Precio), ref productoModel);

            if (ModelState.IsValid)
            {
                try
                {
                    // Seguridad: redondear a 2 decimales
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

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productoModel = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (productoModel == null) return NotFound();

            return View(productoModel);
        }

        // POST: Producto/Delete/5
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

        /// <summary>
        /// Intenta reparar el parseo del precio si llegó con coma o con miles europeos.
        /// Acepta: "05,50", "5,50", "1.234,56" -> 5.50 / 1234.56 (decimal)
        /// </summary>
        private void TryFixPrecioFromForm(string fieldName, ref ProductoModel model)
        {
            // Si ya hay error de "valor no válido" en ese campo, intentamos convertir manualmente
            if (ModelState.TryGetValue(fieldName, out var entry) &&
                entry.Errors.Count > 0)
            {
                var raw = Request.Form[fieldName].ToString();
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    // Normaliza: elimina espacios y separador de miles europeo
                    raw = raw.Trim().Replace(" ", "");
                    // "1.234,56" -> "1234.56"
                    raw = raw.Replace(".", "").Replace(",", ".");

                    if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
                    {
                        // Borramos errores previos y asignamos el valor corregido
                        entry.Errors.Clear();
                        model.Precio = parsed;
                        // Marcamos el valor como intentado con el string corregido
                        ModelState.SetModelValue(fieldName, parsed, parsed.ToString("0.##", CultureInfo.InvariantCulture));
                    }
                }
            }
        }
    }
}
