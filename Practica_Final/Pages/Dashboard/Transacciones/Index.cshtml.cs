using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Contexts;
using Practica_Final.Infrastructure.Repositories;

namespace Practica_Final.Pages.Dashboard.Transacciones
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class IndexModel : PageModel
    {
        public List<HistorialTransaciones> historialTransaciones { get; set; }

        private readonly ApplicationDbContext _dbContext;
        private readonly IRepositoryCuentaBancarias _repositoryCuenta;
        private readonly IRepositoryUsuario _repositoryUsuario;

        public IndexModel(ApplicationDbContext applicationDbContext,
            IRepositoryCuentaBancarias repositoryCuentaBancaria,
            IRepositoryUsuario repositoryUsuario)
        {
            _repositoryCuenta = repositoryCuentaBancaria;
            _repositoryUsuario = repositoryUsuario;
            _dbContext = applicationDbContext;
        }



        public void OnGet(DateTime? inicio, DateTime? fin)
        {
            if (inicio.HasValue && fin.HasValue)
            {
                GetHistoriarFiltrado(inicio, fin);
            }
            else
            {
                GetHistoriar();
            }

        }

        public void GetHistoriar()
        {
            historialTransaciones = _dbContext.Transferencias.Join(
               _dbContext.CuentasBancarias,
               transferencia => transferencia.CuentaBancariaRemitentetarioId,
               cuentaBancaria => cuentaBancaria.Id,
               (transferencia, cuentaBancaria) => new
               {
                   transferencia.Monto,
                   transferencia.Fecha,
                   CuentaBancariaDestinatario = _repositoryCuenta.GetCuentaById(transferencia.CuentaBancariaDestinatarioId),
                   CuentaBancariaRemitente = _repositoryCuenta.GetCuentaById(transferencia.CuentaBancariaRemitentetarioId),
               }
               ).Select(h => new HistorialTransaciones
               {
                   Tipo = (int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)) == h.CuentaBancariaRemitente.UsuarioId) ? "Enviada" : "Recibida",
                   UsuarioRemit = _repositoryUsuario.UsuarioById(h.CuentaBancariaRemitente.UsuarioId),
                   UsuarioDest = _repositoryUsuario.UsuarioById(h.CuentaBancariaDestinatario.UsuarioId),
                   Monto = h.Monto,
                   Fecha = h.Fecha,
                   CuentaBancariaDestinatario = h.CuentaBancariaDestinatario,
                   CuentaBancariaRemitente = h.CuentaBancariaRemitente,
               })
               .ToList();
        }

        public void GetHistoriarFiltrado(DateTime? inicio, DateTime? fin)
        {

            historialTransaciones = _dbContext.Transferencias.Join(
               _dbContext.CuentasBancarias,
               transferencia => transferencia.CuentaBancariaRemitentetarioId,
               cuentaBancaria => cuentaBancaria.Id,
               (transferencia, cuentaBancaria) => new
               {
                   transferencia.Monto,
                   transferencia.Fecha,
                   CuentaBancariaDestinatario = _repositoryCuenta.GetCuentaById(transferencia.CuentaBancariaDestinatarioId),
                   CuentaBancariaRemitente = _repositoryCuenta.GetCuentaById(transferencia.CuentaBancariaRemitentetarioId),
               }
               ).Select(h => new HistorialTransaciones
               {
                   Tipo = (int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)) == h.CuentaBancariaRemitente.UsuarioId) ? "Enviada" : "Recibida",
                   UsuarioRemit = _repositoryUsuario.UsuarioById(h.CuentaBancariaRemitente.UsuarioId),
                   UsuarioDest = _repositoryUsuario.UsuarioById(h.CuentaBancariaDestinatario.UsuarioId),
                   Monto = h.Monto,
                   Fecha = h.Fecha,
                   CuentaBancariaDestinatario = h.CuentaBancariaDestinatario,
                   CuentaBancariaRemitente = h.CuentaBancariaRemitente,
               })
               .Where(h => h.Fecha.Date >= inicio && h.Fecha.Date <= fin)
               .ToList();
        }

        public string GetIniciales()
        {
            string name = User.FindFirstValue(ClaimTypes.GivenName).Substring(0, 1);
            string lastName = User.FindFirstValue(ClaimTypes.Surname).Substring(0, 1);
            return $"{name}{lastName}";
        }
    }



    public class HistorialTransaciones
    {
        public string Tipo { get; set; }
        public Practica_Final.Domain.Entities.Usuario UsuarioRemit { get; set; }
        public Practica_Final.Domain.Entities.Usuario UsuarioDest { get; set; }
        public double Monto { get; set; }
        public DateTime Fecha { get; set; }
        public CuentaBancaria CuentaBancariaDestinatario { get; set; }
        public CuentaBancaria CuentaBancariaRemitente { get; set; }
    }



}
