using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Practica_Final.Infrastructure.Repositories;

namespace PracticaFinal.Pages
{
    [AllowAnonymous]
    public class RegistrarseModel : PageModel
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        [BindProperty]
        public RegisterModel usuario { get; set; }

        public RegistrarseModel(IRepositoryUsuario repositoryUsuario)
        {
            this._repositoryUsuario = repositoryUsuario;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Dashboard");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["passwordDiferente"] = null;
            ViewData["emailEnUso"] = null;
            if (ModelState.IsValid)
            {
                if(usuario.Password.Equals(usuario.ConfirmPassword))
                {
                    bool userExist = await _repositoryUsuario.IsUsuarioExist(usuario.Email);
                    if (userExist)
                    {
                        ViewData["emailEnUso"] = $"El correo {usuario.Email} ya esta en uso";                    
                    }
                    else
                    {
                        await _repositoryUsuario.Register(new Usuario
                        {
                            Nombre = usuario.Nombre,
                            Apellido = usuario.Apellido,
                            Email = usuario.Email,
                            Password = usuario.Password
                        });
                        return RedirectToPage("./Login");
                    }
                }
                else
                {
                    ViewData["passwordDiferente"] = $"Las contraseñas no coinciden";
                }
               
            }

            return Page();
        }
    }



    public class RegisterModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
