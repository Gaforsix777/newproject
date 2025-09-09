using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;

namespace practicamvc.Controllers
{
    public class ClienteModelsController : Controller
    {
        private readonly ArtesaniasDBContext _context;
        public ClienteModelsController(ArtesaniasDBContext context) => _context = context;

        // GET: ClienteModels
        // /ClienteModels?buscar=camila
        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["FiltroActual"] = buscar;
            var q = _context.Clientes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(buscar))
                q = q.Where(c => c.Nombre.Contains(buscar) || (c.Email ?? "").Contains(buscar));
            return View(await q.OrderBy(c => c.Nombre).ToListAsync());
        }

        // GET: ClienteModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .Include(c => c.Pedidos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // GET: ClienteModels/Create
        public IActionResult Create() => View();

        // POST: ClienteModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Direccion")] ClienteModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ClienteModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.Clientes.FindAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: ClienteModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Direccion")] ClienteModel model)
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
                if (!await _context.Clientes.AnyAsync(e => e.Id == model.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ClienteModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        // POST: ClienteModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Clientes.FindAsync(id);
            if (model != null) _context.Clientes.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
