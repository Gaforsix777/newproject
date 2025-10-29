using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;

namespace practicamvc.Controllers
{
    public class DetallePedidoModelsController : Controller
    {
        private readonly ArtesaniasDBContext _context;
        public DetallePedidoModelsController(ArtesaniasDBContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var data = _context.DetallePedidos
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .OrderByDescending(d => d.Id);
            return View(await data.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var detalle = await _context.DetallePedidos
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalle == null) return NotFound();
            return View(detalle);
        }

        public IActionResult Create()
        {
            ViewData["IdPedido"] = new SelectList(_context.Pedidos.OrderByDescending(p => p.Id), "Id", "Id");
            ViewData["IdProducto"] = new SelectList(_context.Productos.OrderBy(p => p.Nombre), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPedido,IdProducto,Cantidad,PrecioUnitario")] DetallePedidoModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IdPedido"] = new SelectList(_context.Pedidos, "Id", "Id", model.IdPedido);
                ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Nombre", model.IdProducto);
                return View(model);
            }
            if (model.PrecioUnitario <= 0)
            {
                var prod = await _context.Productos.FindAsync(model.IdProducto);
                if (prod != null) model.PrecioUnitario = prod.Precio;
            }
            _context.Add(model);
            await _context.SaveChangesAsync();
            await RecalcularTotalPedido(model.IdPedido);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.DetallePedidos.FindAsync(id);
            if (model == null) return NotFound();
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "Id", "Id", model.IdPedido);
            ViewData["IdProducto"] = new SelectList(_context.Productos.OrderBy(p => p.Nombre), "Id", "Nombre", model.IdProducto);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPedido,IdProducto,Cantidad,PrecioUnitario")] DetallePedidoModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["IdPedido"] = new SelectList(_context.Pedidos, "Id", "Id", model.IdPedido);
                ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Nombre", model.IdProducto);
                return View(model);
            }
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                await RecalcularTotalPedido(model.IdPedido);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.DetallePedidos.AnyAsync(e => e.Id == model.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var model = await _context.DetallePedidos
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.DetallePedidos.FindAsync(id);
            if (model != null)
            {
                int idPedido = model.IdPedido;
                _context.DetallePedidos.Remove(model);
                await _context.SaveChangesAsync();
                await RecalcularTotalPedido(idPedido);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task RecalcularTotalPedido(int idPedido)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.DetallePedidos)
                .FirstOrDefaultAsync(p => p.Id == idPedido);
            if (pedido == null) return;
            pedido.MontoTotal = pedido.DetallePedidos.Sum(x => x.Cantidad * x.PrecioUnitario);
            _context.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
