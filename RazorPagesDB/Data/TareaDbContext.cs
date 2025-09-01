using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Models;
using System.Collections.Generic;

namespace RazorPagesDB.Data
{
    public class TareaDbContext : DbContext
    {
        public TareaDbContext(DbContextOptions<TareaDbContext> options) : base(options) { }

        public DbSet<Tarea> Tareas => Set<Tarea>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tarea>().HasData(
                new Tarea { Id = 1, nombreTarea = "Definir backlog", fechaVencimiento = DateTime.Today.AddDays(3), estado = "Pendiente", IdUsuario = 1 },
                new Tarea { Id = 2, nombreTarea = "Diseñar UI Razor", fechaVencimiento = DateTime.Today.AddDays(7), estado = "En Progreso", IdUsuario = 2 },
                new Tarea { Id = 3, nombreTarea = "Publicar a prueba", fechaVencimiento = DateTime.Today.AddDays(10), estado = "Completada", IdUsuario = 1 }
            );
        }
    }
}