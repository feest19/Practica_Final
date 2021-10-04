using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_Final.Infrastructure.Configurations
{
    public class TipoCuentaConfig : IEntityTypeConfiguration<TipoCuenta>
    {
        public void Configure(EntityTypeBuilder<TipoCuenta> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Tipo)
                .IsRequired();
        }
    }
}
