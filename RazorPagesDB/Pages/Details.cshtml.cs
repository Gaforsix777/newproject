using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Data;   
using RazorPagesDB.Models;

namespace RazorPagesDB.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly TareaDbContext _context;
        public DetailsModel(TareaDbContext context) => _context = context;

        public Tarea Tarea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            // No necesitamos tracking para mostrar detalles
            var entity = await _context.Tareas
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();

            Tarea = entity;
            return Page();
        }
    }
}
