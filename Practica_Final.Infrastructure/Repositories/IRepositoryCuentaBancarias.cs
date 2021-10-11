using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Practica_Final.Infrastructure.Repositories
{
    public interface IRepositoryCuentaBancarias
    {
        public Task<bool> Insert(CuentaBancaria cuenta);
        public Task<List<CuentaBancaria>> GetCuentasBancariasByUserId(int userID);
        public Task<Usuario> GetUserByCuentasBancariaID(int cuentaId);

        public string getUsuarioByCuenta(int id);

        public string getTipoCuenta(int id);

        public int getNumeroCuenta(int id);

        public Task Update(CuentaBancaria cuenta);

        public CuentaBancaria GetCuentaByIdCuenta(int cuenta);
        public CuentaBancaria GetCuentaById(int id);
        public bool IsNumeroCuentaExist(int cuenta);

    }
}
