using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Repositories;

namespace Practica_Final.Pages.Dashboard.Cuentas
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public CuentaBancariaModel cuentaBancariaModel { get; set; }
        public List<TipoCuenta> tipoCuentas { get; set; }

        private readonly IRepositoryTipoCuenta _repositoryTipoCuenta;

        private readonly IRepositoryCuentaBancarias _repositoryCuentaBancaria;

        public IndexModel(IRepositoryCuentaBancarias repositoryCuentaBancaria, IRepositoryTipoCuenta repositoryTipoCuenta)
        {
            _repositoryCuentaBancaria = repositoryCuentaBancaria;
            _repositoryTipoCuenta = repositoryTipoCuenta;
        }

        public async Task OnGet()
        {
            this.tipoCuentas = await _repositoryTipoCuenta.GetAllTipoCuenta();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (this.cuentaBancariaModel.Monto > 0)
                {
                    var cuenta = new CuentaBancaria();
                    cuenta.Monto = cuentaBancariaModel.Monto;
                    cuenta.NumeroCuenta = GenerarNumeroCuenta();
                    cuenta.Fecha = DateTime.Now;
                    cuenta.UsuarioId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    cuenta.TipoCuentaId = cuentaBancariaModel.IdTipoCuenta;
                    await this._repositoryCuentaBancaria.Insert(cuenta);
                    cuentaBancariaModel = null;
                    ViewData["cuentaCreada"] = "Cuenta creada correctamente";
                    ViewData["numeroCuenta"] = $"El numero de la cuenta es: {cuenta.NumeroCuenta}";
                    return Page();
                }
            }
            return Page();
        }

        private int GenerarNumeroCuenta()
        {
            Random generator = new Random();
            int numero = generator.Next(100000000, 999999999);
            if (_repositoryCuentaBancaria.IsNumeroCuentaExist(numero))
            {
                numero = GenerarNumeroCuenta();
            }
            return numero;
        }

        public string GetIniciales()
        {
            string name = User.FindFirstValue(ClaimTypes.GivenName).Substring(0, 1);
            string lastName = User.FindFirstValue(ClaimTypes.Surname).Substring(0, 1);
            return $"{name}{lastName}";
        }
    }

    public class CuentaBancariaModel
    {
        [Required]
        public int IdTipoCuenta { get; set; }
        [Required]
        public double Monto { get; set; }
    }

}
