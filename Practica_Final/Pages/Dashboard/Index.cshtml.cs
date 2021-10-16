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
using Practica_Final.Infrastructure.Repositories;
using Practica_Final.Infrastructure.Contexts;

namespace Practica_Final.Pages.Dashboard
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class IndexModel : PageModel
    {
        private readonly IRepositoryCuentaBancarias _repositoryCuentas;
        public string Usuario { get; set; }
        public List<CuentasModel> cuentasModels { get; set; }
        private readonly ApplicationDbContext _Context;


        public  IndexModel(IRepositoryCuentaBancarias CuentasServices, ApplicationDbContext context)
        {
            this._repositoryCuentas = CuentasServices;
            this._Context = context;
        }


        public async Task OnGetAsync()
        {
            int idUsuario;
            bool success = Int32.TryParse(this.User.FindFirstValue(ClaimTypes.NameIdentifier), out idUsuario);
            if (success)
            {
                var result = await _repositoryCuentas.GetCuentasBancariasByUserId(idUsuario);
                this.cuentasModels = _Context.CuentasBancarias.Join(
                        _Context.TipoCuentas,
                        cuentaBancaria => cuentaBancaria.TipoCuentaId,
                        tipoCuenta => tipoCuenta.Id,
                        (cuentaBancaria, tipoCuenta) => new CuentasModel
                        {
                            NumeroCuenta = cuentaBancaria.NumeroCuenta,
                            Tipo = tipoCuenta.Tipo,
                            Monto = cuentaBancaria.Monto,
                            Fecha = cuentaBancaria.Fecha
                            
                        }
                        ).ToList();
            }
        }

        public string GetIniciales()
        {
            string name = User.FindFirstValue(ClaimTypes.GivenName).Substring(0,1);
            string lastName = User.FindFirstValue(ClaimTypes.Surname).Substring(0,1);
            return $"{name}{lastName}";
        }

    }

    public class CuentasModel
    {
        public int NumeroCuenta { get; set; }
        public string Tipo { get; set; }
        public double Monto { get; set; }
        public DateTime Fecha { get; set; }
    }


}
