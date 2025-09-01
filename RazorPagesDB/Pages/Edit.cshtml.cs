using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Data;          // <- si tu DbContext está en RazorPagesDB.data, cambia esta línea por RazorPagesDB.data
using RazorPagesDB.Models;

namespace RazorPagesDB.Pages
{
    public class EditModel : PageModel
    {
        private readonly TareaDbContext _context;
        public EditModel(TareaDbContext context) => _context = context;

        [BindProperty]
        public Tarea Tarea { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            // Sin tracking para leer (mejor performance)
            var entity = await _context.Tareas
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();

            Tarea = entity;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Adjuntar el modelo editado y marcarlo como modificado
            _context.Attach(Tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si otro proceso borró la entidad, devolvemos 404
                var stillExists = await _context.Tareas.AnyAsync(e => e.Id == Tarea.Id);
                if (!stillExists) return NotFound();

                // Si fue otro tipo de conflicto, relanzamos
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
