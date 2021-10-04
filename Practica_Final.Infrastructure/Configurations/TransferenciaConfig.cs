using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_Final.Infrastructure.Configurations
{
    public class TransferenciaConfig : IEntityTypeConfiguration<Transferencia>
    {
        public void Configure(EntityTypeBuilder<Transferencia> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.CuentaBancariaDestinatario).
                WithMany().
                HasForeignKey(u => u.CuentaBancariaDestinatarioId);
            builder.HasOne(t => t.CuentaBancariaRemitente).
               WithMany().
               HasForeignKey(u => u.CuentaBancariaRemitentetarioId);
            builder.Property(t => t.Monto)
                .IsRequired();
            builder.Property(t => t.Estado);
            builder.Property(t => t.Fecha);
        }
    }
}
