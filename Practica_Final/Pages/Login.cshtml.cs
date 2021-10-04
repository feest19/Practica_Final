using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Contexts;

namespace PracticaFinal.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
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
            string email = Request.Form["email"];
            string pass = Request.Form["password"];
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email.Equals(email) && u.Password.Equals(pass));
            if (usuario is not null)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));
                identity.AddClaim(new Claim(ClaimTypes.Surname, usuario.Apellido));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                   new AuthenticationProperties { IsPersistent = true });
                return Redirect("/Dashboard");
            }
            ViewData["validacion"] = $"Email o contraseña incorrecta";
            return Page();

        }


    }
}
