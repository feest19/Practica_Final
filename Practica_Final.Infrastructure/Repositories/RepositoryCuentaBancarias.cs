using Microsoft.EntityFrameworkCore;
using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica_Final.Infrastructure.Repositories
{
    public class RepositoryCuentaBancarias : IRepositoryCuentaBancarias
    {
        private readonly Contexts.ApplicationDbContext _context;
        //TODO: Hacer la inyeccion de dependencia. Refactorizar el codigo
        public RepositoryCuentaBancarias(Contexts.ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<List<CuentaBancaria>> GetCuentasBancariasByUserId(int usuario) => await _context.CuentasBancarias.Where(x => x.UsuarioId == usuario).ToListAsync();

        public async Task<Usuario> GetUserByCuentasBancariaID(int cuentaId)
        {
            Usuario usuario;
            try
            {
                var query =  from c in _context.CuentasBancarias
                            where c.Id == cuentaId
                            select c.Usuario;
                usuario = await query.FirstAsync();
            }
            catch (Exception)
            {
                usuario = null;
                throw;
            }
            return usuario;
        }

    
    }
}
