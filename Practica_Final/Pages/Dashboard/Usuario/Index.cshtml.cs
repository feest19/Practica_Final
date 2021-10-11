using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Practica_Final.Infrastructure.Repositories;

namespace Practica_Final.Pages.Dashboard.Usuario
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class IndexModel : PageModel
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        [BindProperty]
        public UsuarioModel usuarioModel { get; set; } = new UsuarioModel();

        public IndexModel(IRepositoryUsuario repositoryUsuario)
        {
            this._repositoryUsuario = repositoryUsuario;
        }

        public async Task OnGet()
        {
            var usuario = await GetUsuario();
            usuarioModel.Nombre = usuario.Nombre;
            usuarioModel.Apellido = usuario.Apellido;
            usuarioModel.Email = usuario.Email;
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var usuario = await GetUsuario();
                bool userExist = await _repositoryUsuario.IsUsuarioExist(usuarioModel.Email);
                if (userExist && !usuarioModel.Email.Equals(usuario.Email))
                {
                    ViewData["validacion"] = $"El correo {usuarioModel.Email} ya esta en uso";
                }
                else
                {
                    usuario.Nombre = usuarioModel.Nombre;
                    usuario.Apellido = usuarioModel.Apellido;
                    usuario.Email = usuarioModel.Email;
                    await _repositoryUsuario.Update(usuario);
                    ViewData["isSuccess"] = "Usuario Actualizado correctamente";
                }
            }
        }

        private async Task<Practica_Final.Domain.Entities.Usuario> GetUsuario()
        {
            var idUsuario = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var usuario = await _repositoryUsuario.GetUsuarioById(idUsuario);
            return usuario;
        }
    }

    public class UsuarioModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
    }
}
