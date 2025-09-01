using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Data;
using RazorPagesDB.Interfaces;
using RazorPagesDB.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<TareaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el servicio de inicialización de la base de datos
builder.Services.AddScoped<IDbInitializer, TareaSeeder>();

var app = builder.Build();

// Llama al método de inicialización de la base de datos
SeedDatabase();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        initializer.Initialize(scope.ServiceProvider);
    }
}
