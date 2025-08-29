using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesDB.Models;
using RazorPagesDB.data;

namespace RazorPagesDB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesDB.data.TareaDbContext _context;

        public IndexModel(RazorPagesDB.data.TareaDbContext context)
        {
            _context = context;
        }

        public IList<Tarea> Tarea { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Tarea = await _context.Tareas.ToListAsync();
        }
    }
}
