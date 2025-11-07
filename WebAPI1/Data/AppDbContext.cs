using Microsoft.EntityFrameworkCore;
using WebAPI1.Models;

namespace WebAPI1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
