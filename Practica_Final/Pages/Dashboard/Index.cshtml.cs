using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Practica_Final.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Practica_Final.Pages.Dashboard
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class IndexModel : PageModel
    {
        private readonly Infrastructure.Contexts.ApplicationDbContext _context;

        public IndexModel(Infrastructure.Contexts.ApplicationDbContext context)
        {
            _context = context;
        }
        public List<CuentaBancaria> Cuentas { get; set; } 
        public List<Transferencia> transferencias { get; set; }
        public string Usuario { get; set; }
        //public void OnGet()
        //{
        //    string idUsuario = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
           
        //}
        public async Task OnGetAsync()
        {
            int idUsuario;
            bool success = Int32.TryParse(this.User.FindFirstValue(ClaimTypes.NameIdentifier), out idUsuario);

            if (success)
            {
                var historyTransf = from c in _context.CuentasBancarias
                                  join t in _context.Transferencias on c.Id equals t.CuentaBancariaDestinatarioId 
                              where c.UsuarioId == idUsuario
                              orderby t.Fecha descending
                              select t;
              this.Cuentas = await _context.CuentasBancarias.Where(x => x.UsuarioId == idUsuario).ToListAsync();
                this.transferencias = await historyTransf.Take(5).ToListAsync();
            }
        }
    }
}
