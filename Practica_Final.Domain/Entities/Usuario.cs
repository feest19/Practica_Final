using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_Final.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Password { get; set; }
    }
}
