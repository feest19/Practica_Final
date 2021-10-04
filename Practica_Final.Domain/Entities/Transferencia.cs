using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_Final.Domain.Entities
{
    public class Transferencia : BaseEntity
    {
        public int Estado { get; set; }
        public int CuentaBancariaDestinatarioId { get; set; }
        public int CuentaBancariaRemitentetarioId { get; set; }
        public double Monto { get; set; }
        public DateTime Fecha { get; set; }
        public CuentaBancaria CuentaBancariaDestinatario { get; set; }
        public CuentaBancaria CuentaBancariaRemitente { get; set; }
    }
}
