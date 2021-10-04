using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_Final.Infrastructure.Configurations
{
    public class CuentaBancariaConfig : IEntityTypeConfiguration<CuentaBancaria>
    {
        public void Configure(EntityTypeBuilder<CuentaBancaria> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.Usuario).
                WithMany().
                HasForeignKey(u => u.UsuarioId);
            builder.Property(c => c.UsuarioId)
              .IsRequired();
            builder.HasIndex(c => c.NumeroCuenta)
                .IsUnique();
            builder.Property(c => c.NumeroCuenta)
                .IsRequired();
            builder.HasOne(c => c.TipoCuenta).
                WithMany().
                HasForeignKey(u => u.TipoCuentaId);
            builder.Property(c => c.Monto)
                .IsRequired();
        }
    }
}
