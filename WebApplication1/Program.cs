using Microsoft.EntityFrameworkCore;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la base de datos con Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuraci�n de autenticaci�n con cookies
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Ruta de login
        options.LogoutPath = "/Account/Logout"; // Ruta de logout
        options.AccessDeniedPath = "/Home/AccessDenied"; // Ruta de acceso denegado
    });

// Agregar servicios de MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuraci�n del middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Habilitar la autenticaci�n
app.UseAuthorization();  // Habilitar autorizaci�n

// Configuraci�n de las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
