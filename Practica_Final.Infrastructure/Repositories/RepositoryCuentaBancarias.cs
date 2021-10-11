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

        public async Task Update(CuentaBancaria cuenta)
        {
            _context.Attach(cuenta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public CuentaBancaria GetCuentaByIdCuenta(int cuenta)
        {
            return this._context.CuentasBancarias.FirstOrDefault(c => c.NumeroCuenta == cuenta);
        }

        public CuentaBancaria GetCuentaById(int id)
        {
            return this._context.CuentasBancarias.FirstOrDefault(c => c.Id == id);
        }

        public async Task<bool> Insert(CuentaBancaria cuenta)
        {
            await _context.CuentasBancarias.AddAsync(cuenta);
            int isSuccess = await _context.SaveChangesAsync();
            return isSuccess > 0;
        }

        public bool IsNumeroCuentaExist(int cuenta)
        {
            bool isExiste = false;
            try
            {
                var usuario = this._context.CuentasBancarias.FirstOrDefault(c => c.NumeroCuenta == cuenta);
                isExiste = usuario != null;
            }
            catch (Exception)
            {
                isExiste = false;
            }
            return isExiste;
        }
    }
}
