using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_Final.Infrastructure.Configurations
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email)
               .IsRequired();
            builder.HasIndex(c => c.Email)
               .IsUnique();
            builder.Property(u => u.Nombre)
              .IsRequired();
            builder.Property(u => u.Apellido)
              .IsRequired();
            builder.Property(u => u.Password)
              .IsRequired();
        }
    }
}
