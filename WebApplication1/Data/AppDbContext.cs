using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class AppDbContext : IdentityDbContext<User>  // Cambia DbContext por IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets para los modelos
        public DbSet<Product> Products => Set<Product>();
    }
}
