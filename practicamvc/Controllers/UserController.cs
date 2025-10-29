using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;
using practicamvc.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;

namespace practicamvc.Controllers
{
    public class UserController : Controller
    {
        private readonly ArtesaniasDBContext _db;

        public UserController(ArtesaniasDBContext db)
        {
            _db = db;
        }

        private (string Hash, string Salt) HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string saltB64 = Convert.ToBase64String(salt);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password, salt, KeyDerivationPrf.HMACSHA256, 100000, 256 / 8));
            return (hash, saltB64);
        }

        private bool VerifyPassword(string password, string saltB64, string expectedHash)
        {
            byte[] salt = Convert.FromBase64String(saltB64);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password, salt, KeyDerivationPrf.HMACSHA256, 100000, 256 / 8));
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(hash),
                Convert.FromBase64String(expectedHash));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == vm.UserOrEmail || u.Email == vm.UserOrEmail);

            if (user == null || !user.IsActive || !VerifyPassword(vm.Password, user.Salt, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = vm.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

            if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.UserName.Contains(' '))
            {
                ModelState.AddModelError(nameof(vm.UserName), "El nombre de usuario no puede contener espacios.");
                return View(vm);
            }

            bool exists = await _db.Users.AnyAsync(u => u.UserName == vm.UserName || u.Email == vm.Email);
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Usuario o email ya existe.");
                return View(vm);
            }

            var (hash, salt) = HashPassword(vm.Password);

            var user = new UserModel
            {
                UserName = vm.UserName,
                Email = vm.Email,
                PasswordHash = hash,
                Salt = salt,
                Role = vm.Role,
                IsActive = true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Cuenta creada, ahora inicia sesión.";
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
