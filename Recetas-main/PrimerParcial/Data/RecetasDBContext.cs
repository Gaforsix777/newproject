using Microsoft.EntityFrameworkCore;
using PrimerParcial.Models;

namespace PrimerParcial.Data
{
    public class RecetasDBContext : DbContext
    {
        public RecetasDBContext(DbContextOptions<RecetasDBContext> options)
            : base(options)
        {
        }

        // Tablas
        public DbSet<Category> Categories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category (1) -> Recipes (N)
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // evita borrado en cascada de categorías

            // Recipe (1) -> Ingredients (N)
            modelBuilder.Entity<Ingredient>()
                .HasOne(i => i.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // al borrar Recipe, borra sus Ingredients
        }
    }
}
