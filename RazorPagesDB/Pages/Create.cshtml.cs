using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesDB.Data;
using RazorPagesDB.Models;
namespace RazorPagesDB.Pages
{
    public class CreateModel : PageModel
    {
        private readonly TareaDbContext _context;

        public CreateModel(TareaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tarea Tarea { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Tareas.Add(Tarea);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}