using Microsoft.EntityFrameworkCore;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos con Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de autenticación con cookies
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

// Configuración del middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Habilitar la autenticación
app.UseAuthorization();  // Habilitar autorización

// Configuración de las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
