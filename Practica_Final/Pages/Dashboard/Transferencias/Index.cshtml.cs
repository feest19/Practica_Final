using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Repositories;

namespace Practica_Final.Pages.Dashboard.Transferencias
{
    public class IndexModel : PageModel
    {
        public List<NumerosCuentasModel> listaNumerosCuentas { get; set; }

        [BindProperty]
        public TransferenciaModel transferenciaModel { get; set; }

        private readonly IRepositoryTransferencia _transferenciaRepository;

        private readonly IRepositoryCuentaBancarias _repositoryCuentaBancaria;

        public IndexModel(IRepositoryCuentaBancarias repositoryCuentaBancaria, IRepositoryTransferencia transferenciaRepository)
        {
            _repositoryCuentaBancaria = repositoryCuentaBancaria;
            _transferenciaRepository = transferenciaRepository;
        }

        public async Task OnGetAsync()
        {
            await GetNumerosCuentas();
        }

        private async Task GetNumerosCuentas()
        {
            int idUsuario = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await this._repositoryCuentaBancaria.GetCuentasBancariasByUserId(idUsuario);
            listaNumerosCuentas = result.Where(x => x.Monto > 0).Select(c => new NumerosCuentasModel
            {
                IdNumeroCuenta = c.Id,
                NumeroCuenta = c.NumeroCuenta
            }).ToList();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var cuentaRemitente = this._repositoryCuentaBancaria.
                    GetCuentaByIdCuenta(this.transferenciaModel.NumeroCuentaRemitente);

                var cuentaDestinatario = this._repositoryCuentaBancaria.
                    GetCuentaByIdCuenta(this.transferenciaModel.NumeroCuentaDestinatario);

                if (cuentaDestinatario is not null)
                {
                    if (cuentaRemitente.NumeroCuenta == cuentaDestinatario.NumeroCuenta)
                    {
                        ViewData["validacion"] = "La cuenta destinatario no puede ser igual a la cuenta remitente";
                    }
                    if (transferenciaModel.Monto > cuentaRemitente.Monto)
                    {
                        ViewData["validacion"] = "El monto a transferir no puede ser mayor al monto disponible en su cuenta";
                    }
                    else
                    {
                        cuentaRemitente.Monto -= transferenciaModel.Monto;
                        cuentaDestinatario.Monto += transferenciaModel.Monto;
                        await this._repositoryCuentaBancaria.Update(cuentaRemitente);
                        await this._repositoryCuentaBancaria.Update(cuentaDestinatario);

                        var transferencia = new Transferencia();
                        transferencia.Estado = 1;
                        transferencia.CuentaBancariaRemitentetarioId = cuentaRemitente.Id;
                        transferencia.CuentaBancariaDestinatarioId = cuentaDestinatario.Id;
                        transferencia.Monto = transferenciaModel.Monto;
                        transferencia.Fecha = DateTime.Now;
                        await _transferenciaRepository.Insert(transferencia);
                        ViewData["isSuccess"] = "Transferencia realizada con correctamente";
                    }
                }
                else
                {
                    ViewData["validacion"] = "El numero de cuenta del destinatario no existe";
                }
            }
            await GetNumerosCuentas();
        }
    }

    public class NumerosCuentasModel
    {
        public int IdNumeroCuenta { get; set; }
        public int NumeroCuenta { get; set; }
    }

    public class TransferenciaModel
    {
        [Required]
        public int NumeroCuentaRemitente { get; set; }
        [Required]
        public int NumeroCuentaDestinatario { get; set; }
        [Required]
        public double Monto { get; set; }
    }



}
