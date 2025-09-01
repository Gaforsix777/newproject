using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Data;
using RazorPagesDB.Models;

namespace RazorPagesDB.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly TareaDbContext _context;
        public DeleteModel(TareaDbContext context) => _context = context;

        [BindProperty]
        public Tarea Tarea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var entity = await _context.Tareas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entity == null) return NotFound();

            Tarea = entity;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var entity = await _context.Tareas.FindAsync(id);
            if (entity != null)
            {
                _context.Tareas.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
