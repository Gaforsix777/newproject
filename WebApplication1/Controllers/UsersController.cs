using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher = new();

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string? q)
        {
            var qry = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
                qry = qry.Where(u => u.Nombre.Contains(q) || u.Email.Contains(q));

            var list = await qry.OrderBy(u => u.Nombre).ToListAsync();
            return View(list);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create() => View(new UserCreateVM());

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Email único
            var exists = await _context.Users.AnyAsync(u => u.Email == vm.Email);
            if (exists)
            {
                ModelState.AddModelError("Email", "Ya existe un usuario con este correo.");
                return View(vm);
            }

            var user = new User
            {
                Nombre = vm.Nombre,
                Email = vm.Email,
                Rol = vm.Rol
            };
            user.PasswordHash = _hasher.HashPassword(user, vm.Password);

            _context.Add(user);
            await _context.SaveChangesAsync();
            TempData["msg"] = "Usuario creado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var vm = new UserEditVM
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Email = user.Email,
                Rol = user.Rol
            };
            return View(vm);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditVM vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Si cambia email, validar que no esté en uso por otro
            if (!string.Equals(user.Email, vm.Email, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _context.Users.AnyAsync(u => u.Email == vm.Email && u.Id != id);
                if (exists)
                {
                    ModelState.AddModelError("Email", "Ya existe un usuario con este correo.");
                    return View(vm);
                }
            }

            user.Nombre = vm.Nombre;
            user.Email = vm.Email;
            user.Rol = vm.Rol;

            if (!string.IsNullOrWhiteSpace(vm.NewPassword))
            {
                user.PasswordHash = _hasher.HashPassword(user, vm.NewPassword);
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["msg"] = "Usuario actualizado.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(vm.Id)) return NotFound();
                throw;
            }
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["msg"] = "Usuario eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id) => _context.Users.Any(e => e.Id == id);
    }
}
