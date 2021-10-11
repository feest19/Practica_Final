using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Practica_Final.Infrastructure.Repositories
{
    public interface IRepositoryTipoCuenta
    {
        public Task<bool> Insert(TipoCuenta tipoCuenta);
        public Task<List<TipoCuenta>> GetAllTipoCuenta();
        public bool TipoCuentaExist(string tipo);
    }
}
