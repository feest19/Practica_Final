using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practica_Final.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {

        public IndexModel()
        {

        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Dashboard");
            }
            return RedirectToPage("./Login");
        }
    }
}
