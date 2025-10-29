using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using practicamvc.Data;
using practicamvc.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ArtesaniasDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/User/Login";
        o.LogoutPath = "/User/Logout";
        o.AccessDeniedPath = "/User/AccessDenied";
        o.Cookie.Name = "artesanias.auth";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromDays(7);
        o.Cookie.HttpOnly = true;
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloClientes", p => p.RequireRole(UserModel.RolCliente));
    options.AddPolicy("SoloProveedores", p => p.RequireRole(UserModel.RolProveedor));
    options.AddPolicy("ClientesYProveedores", p => p.RequireRole(UserModel.RolCliente, UserModel.RolProveedor));
});

var app = builder.Build();

var culture = new CultureInfo("es-BO");
culture.NumberFormat.NumberDecimalSeparator = ",";
culture.NumberFormat.NumberGroupSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ===== SEED =====
static (string Hash, string Salt) HashPassword(string password)
{
    byte[] salt = System.Security.Cryptography.RandomNumberGenerator.GetBytes(128 / 8);
    string saltB64 = Convert.ToBase64String(salt);
    string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password, salt, KeyDerivationPrf.HMACSHA256, 100000, 256 / 8));
    return (hash, saltB64);
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArtesaniasDBContext>();
    await db.Database.MigrateAsync();

    if (!await db.Users.AnyAsync())
    {
        var (hc, sc) = HashPassword("Cliente123!");
        var (hp, sp) = HashPassword("Proveedor123!");

        db.Users.AddRange(
            new UserModel
            {
                UserName = "cliente_demo",
                Email = "cliente@demo.com",
                PasswordHash = hc,
                Salt = sc,
                Role = UserModel.RolCliente,
                IsActive = true
            },
            new UserModel
            {
                UserName = "proveedor_demo",
                Email = "proveedor@demo.com",
                PasswordHash = hp,
                Salt = sp,
                Role = UserModel.RolProveedor,
                IsActive = true
            }
        );
        await db.SaveChangesAsync();
    }
}

app.Run();
