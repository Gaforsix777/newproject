using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Data;

namespace RazorPagesDB
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TareaDbContext>
    {
        public TareaDbContext CreateDbContext(string[] args)
        {
            // Construye la configuración leyendo el appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Obtiene la cadena de conexión
            var builder = new DbContextOptionsBuilder<TareaDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configura el DbContext para usar SQL Server
            builder.UseSqlServer(connectionString);

            // Retorna una nueva instancia del DbContext
            return new TareaDbContext(builder.Options);
        }
    }
}
