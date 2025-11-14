using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly InventarioContext _context;

        public ProductosController(InventarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos() =>
            Ok(await _context.Productos.ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductoById(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto([FromBody] Producto prod)
        {
            _context.Productos.Add(prod);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductoById), new { id = prod.Id }, prod);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarProducto(int id, [FromBody] Producto prod)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            producto.Nombre = prod.Nombre;
            producto.DescripcionCorta = prod.DescripcionCorta;
            producto.Precio = prod.Precio;
            producto.Stock = prod.Stock;
            producto.IdCategoria = prod.IdCategoria;
            producto.IdProveedor = prod.IdProveedor;
            await _context.SaveChangesAsync();
            return Ok(producto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("por-categoria/{idCategoria}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorCategoria(int idCategoria)
        {
            var productos = await _context.Productos
                .Where(p => p.IdCategoria == idCategoria)
                .ToListAsync();
            return Ok(productos);
        }

        [HttpGet("buscar/{nombre}")]
        public async Task<ActionResult<IEnumerable<Producto>>> BuscarProductosPorNombre(string nombre)
        {
            var productos = await _context.Productos
                .Where(p => EF.Functions.Like(p.Nombre, $"%{nombre}%"))
                .ToListAsync();
            return Ok(productos);
        }

        [HttpGet("por-proveedor/{idProveedor}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorProveedor(int idProveedor)
        {
            var productos = await _context.Productos
                .Where(p => p.IdProveedor == idProveedor)
                .ToListAsync();
            return Ok(productos);
        }
    }
}
