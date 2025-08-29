using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Models;
using System.Collections.Generic;

namespace RazorPagesDB.data
{
    public class TareaDbContext : DbContext
    {
        public TareaDbContext(DbContextOptions<TareaDbContext> options) : base(options)
        {

        }
        public DbSet<Tarea> Tareas { get; set; }

        protected TareaDbContext()
        {

        }
    }
}