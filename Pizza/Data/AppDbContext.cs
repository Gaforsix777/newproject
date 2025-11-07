using Pizza.Models;
using Microsoft.EntityFrameworkCore;

namespace Pizza.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users => Set<User>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
