using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Repositories;

namespace Practica_Final.Pages.Dashboard.TipoCuentas
{
    [Authorize(AuthenticationSchemes ="Cookies")]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public TipoCuentaModel tipoCuentasModel { get; set; }

        private readonly IRepositoryTipoCuenta _repositoryTipoCuenta;

        public IndexModel(IRepositoryTipoCuenta repositoryTipoCuenta)
        {
            this._repositoryTipoCuenta = repositoryTipoCuenta;
        }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
              
                if (_repositoryTipoCuenta.TipoCuentaExist(tipoCuentasModel.tipo))
                {
                    ViewData["validacion"] = $"El tipo de cuenta ${tipoCuentasModel.tipo} ya existe";
                }
                else
                {
                    var tipoCuenta = new TipoCuenta
                    {
                        Tipo = tipoCuentasModel.tipo
                    };
                    await _repositoryTipoCuenta.Insert(tipoCuenta);
                    ViewData["isSuccess"] = "Tipo de cuenta creado correctamente";
                }
            }
        }
    }

    public class TipoCuentaModel
    {
        [Required]
        public string tipo { get; set; }
    }

}
