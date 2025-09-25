using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db) { _db = db; }

        // ---------- REGISTER ----------
        [HttpGet]
        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var exists = await _db.Users.AnyAsync(u => u.Email == vm.Email);
            if (exists)
            {
                ModelState.AddModelError(nameof(vm.Email), "Ya existe un usuario con este email.");
                return View(vm);
            }

            var user = new User
            {
                Nombre = vm.Nombre,
                Email = vm.Email,
                Rol = vm.Rol,
                PasswordHash = PasswordHelper.Hash(vm.Password) // guardamos hash "salt$hash"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // (Opcional) loguear automáticamente después de registrar:
            await SignInAsync(user);

            return RedirectToAction("Index", "Home");
        }

        // ---------- LOGIN ----------
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == vm.Email);
            if (user == null || !PasswordHelper.Verify(user.PasswordHash, vm.Password))
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View(vm);
            }

            await SignInAsync(user);

            return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction("Index", "Home");
        }

        // ---------- LOGOUT ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Rol.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        // ---------- ViewModels ----------
        public class RegisterVM
        {
            [Required, StringLength(100)] public string Nombre { get; set; } = string.Empty;
            [Required, EmailAddress, StringLength(200)] public string Email { get; set; } = string.Empty;
            [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
            [Required] public UserRole Rol { get; set; } = UserRole.cliente;
        }

        public class LoginVM
        {
            [Required, EmailAddress] public string Email { get; set; } = string.Empty;
            [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        }
    }
}
