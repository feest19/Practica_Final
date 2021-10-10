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

namespace Practica_Final.Pages.Dashboard
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class IndexModel : PageModel
    {
        private readonly IRepositoryCuentaBancarias _repositoryCuentas;
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryTransferencia _repositoryTransferencia;

        public  IndexModel(IRepositoryTransferencia transferenciaServices, IRepositoryUsuario UserServices,IRepositoryCuentaBancarias CuentasServices)
        {
            this._repositoryCuentas = CuentasServices;
            this._repositoryUsuario = UserServices;
            this._repositoryTransferencia = transferenciaServices;
            this.Cuentas = new List<CuentaBancaria>();
            this.transferencias = new List<Transferencia>();
        }
        public IList<CuentaBancaria> Cuentas { get; set; } 
        public IList<Transferencia> transferencias { get; set; }
        public string Usuario { get; set; }
        
        public async Task OnGetAsync()
        {
            int idUsuario;
            bool success = Int32.TryParse(this.User.FindFirstValue(ClaimTypes.NameIdentifier), out idUsuario);

            if (success)
            {
                this.Cuentas = await _repositoryCuentas.GetCuentasBancariasByUserId(idUsuario);
                this.transferencias =await _repositoryTransferencia.getTransferenciaByCuentaId(idUsuario);
            }
        }

        #region method
 
        #endregion
    }
}
