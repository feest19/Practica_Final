using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_Final.Domain.Entities
{
    public class CuentaBancaria : BaseEntity
    {
        public int UsuarioId { get; set; }
        public int TipoCuentaId { get; set; }
        public int NumeroCuenta { get; set; }
        public double Monto { get; set; }
        public DateTime Fecha { get; set; }
        public Usuario Usuario { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
    }
}
