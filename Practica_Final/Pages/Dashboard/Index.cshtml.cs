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
        public IList<CuentaBancaria> Cuentas { get; set; }
        public IList<Transferencia> transferencias { get; set; }
        public string Usuario { get; set; }
        public List<CuentasModel> cuentasModels { get; set; }
        private readonly ApplicationDbContext _Context;

        public  IndexModel(IRepositoryCuentaBancarias CuentasServices, ApplicationDbContext context)
        {
            this._repositoryCuentas = CuentasServices;
            _Context = context;
            this.Cuentas = new List<CuentaBancaria>();
        }


        public async Task OnGetAsync()
        {
            int idUsuario;
            bool success = Int32.TryParse(this.User.FindFirstValue(ClaimTypes.NameIdentifier), out idUsuario);

            if (success)
            {
                this.Cuentas = await _repositoryCuentas.GetCuentasBancariasByUserId(idUsuario);
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
                            
                        }
                        ).ToList();
            }
        }

        #region method
        public string getTipoCuenta(int tipo) => _repositoryCuentas.getTipoCuenta(tipo);
        #endregion
    }

    public class CuentasModel
    {
        public int NumeroCuenta { get; set; }
        public string Tipo { get; set; }
        public double Monto { get; set; }
        //public DateTime Fecha { get; set; }
    }


}
