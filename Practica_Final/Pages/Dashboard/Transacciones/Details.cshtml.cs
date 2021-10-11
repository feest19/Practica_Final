using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Practica_Final.Domain.Entities;

namespace Practica_Final.Pages.Dashboard.Transacciones
{
    public class DetailsModel : PageModel
    {
        private readonly Infrastructure.Contexts.ApplicationDbContext _context;
        public Transferencia Transferencia { get; set; }


        public DetailsModel(Infrastructure.Contexts.ApplicationDbContext context)
        {
            this._context = context;
        }
        //public void OnGet()
        //{
        //}
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();
            Transferencia = await _context.Transferencias.SingleOrDefaultAsync(x => x.Id == id);
            if (Transferencia == null)
                return NotFound();
            return Page();
        }
    }
}
