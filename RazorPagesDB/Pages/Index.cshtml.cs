using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Models;
using RazorPagesDB.Data;

namespace RazorPagesDB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TareaDbContext _context;
        public IndexModel(TareaDbContext context) => _context = context;

        public IList<Tarea> Tarea { get; set; } = new List<Tarea>();

        public async Task OnGetAsync()
        {
            Tarea = await _context.Tareas
                .OrderBy(t => t.fechaVencimiento)
                .ToListAsync();
        }

        // Helper para badges en la vista
        public string GetEstadoBadge(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado)) return "badge bg-secondary-subtle";
            return estado.Trim().ToLower() switch
            {
                "pendiente" => "badge bg-warning-subtle",
                "en progreso" => "badge bg-info-subtle",
                "completada" => "badge bg-success-subtle",
                _ => "badge bg-secondary-subtle"
            };
        }
    }
}