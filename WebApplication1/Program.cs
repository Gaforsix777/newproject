using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1;  // Usa tu namespace

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Identity y DBContext para que funcione correctamente
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()  // Este método se usa para asociar Identity con tu DbContext
    .AddDefaultTokenProviders();  // Para generar tokens como en el reset de contraseñas

builder.Services.AddControllersWithViews(); // Si estás usando MVC

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Habilitar autenticación
app.UseAuthorization();   // Habilitar autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
