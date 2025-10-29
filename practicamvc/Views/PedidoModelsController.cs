using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;

namespace practicamvc.Controllers
{
    [Authorize(Policy = "SoloClientes")]
    public class PedidoModelsController : Controller
    {
        private readonly ArtesaniasDBContext _context;
        public PedidoModelsController(ArtesaniasDBContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var data = _context.Pedidos
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.FechaPedido);
            return View(await data.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallePedidos)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null) return NotFound();
            return View(pedido);
        }

        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes.OrderBy(c => c.Nombre), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaPedido,IdCliente,Direccion")] PedidoModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nombre", model.IdCliente);
                return View(model);
            }
            model.MontoTotal = 0m;
            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.Pedidos.FindAsync(id);
            if (model == null) return NotFound();
            ViewData["IdCliente"] = new SelectList(_context.Clientes.OrderBy(c => c.Nombre), "Id", "Nombre", model.IdCliente);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaPedido,IdCliente,Direccion,MontoTotal")] PedidoModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nombre", model.IdCliente);
                return View(model);
            }
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Pedidos.AnyAsync(e => e.Id == model.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Pedidos.FindAsync(id);
            if (model != null) _context.Pedidos.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
