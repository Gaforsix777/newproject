using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesDB.Models;
using RazorPagesDB.data;

namespace RazorPagesDB.Pages
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesDB.data.TareaDbContext _context;

        public CreateModel(RazorPagesDB.data.TareaDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tarea Tarea { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Tareas.Add(Tarea);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
