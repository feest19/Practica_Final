using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Practica_Final.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Practica_Final.Infrastructure.Repositories;

namespace Practica_Final.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                    b => 
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });
            services.AddTransient<IRepositoryUsuario,RepositoryUsuario>();
        }
    }
}
