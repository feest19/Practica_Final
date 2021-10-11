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

        public string getUsuarioByCuenta(int id)
        {
            var query = from u in _context.Usuarios
                        join c in _context.CuentasBancarias on u.Id equals c.UsuarioId
                        where c.Id == id
                        select u;
            var usuarioName = String.Format($"{query.First().Nombre} {query.First().Apellido}");

            return usuarioName;
        }

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

        public string getTipoCuenta(int id)
        {
            return _context.TipoCuentas.FirstOrDefault(t => t.Id == id).Tipo;
        }

        public int getNumeroCuenta(int id)
        {
            return _context.CuentasBancarias.FirstOrDefault(c=> c.Id == id).NumeroCuenta;
        }
    }
}
