using Microsoft.EntityFrameworkCore;
using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica_Final.Infrastructure.Repositories
{
    public class RepositoryTipoCuenta : IRepositoryTipoCuenta
    {
        private readonly ApplicationDbContext _context;

        public RepositoryTipoCuenta(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<TipoCuenta>> GetAllTipoCuenta()
        {
            var cuentas = await _context.TipoCuentas.ToListAsync();
            return cuentas;
        }

        public async Task<bool> Insert(TipoCuenta tipoCuenta)
        {
            await this._context.TipoCuentas.AddAsync(tipoCuenta);
            int isSuccess = await _context.SaveChangesAsync();
            return isSuccess > 0;
        }

        public bool TipoCuentaExist(string tipo)
        {
            bool isExiste = false;
            try
            {
                var usuario = this._context.TipoCuentas.FirstOrDefault(u => u.Tipo.Equals(tipo));
                isExiste = usuario == null ? false : true;
            }
            catch (Exception)
            {
                isExiste = false;
            }
            return isExiste;
        }
    }
}
