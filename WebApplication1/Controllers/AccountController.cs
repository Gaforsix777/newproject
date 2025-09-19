using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<User> _hasher = new();

        public AccountController(AppDbContext db) { _db = db; }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == vm.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View(vm);
            }

            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, vm.Password);
            if (res == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Rol.ToString()) // admin|cliente|empleado
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Denied() => View();

        public class LoginVM
        {
            [Required, EmailAddress] public string Email { get; set; } = string.Empty;
            [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        }
    }
}
