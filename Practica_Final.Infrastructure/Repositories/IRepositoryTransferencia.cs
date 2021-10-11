using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Practica_Final.Infrastructure.Repositories
{
    public interface IRepositoryTransferencia
    {
        public Task<List<Transferencia>> getTransferenciaByCuentaId(int cuentaID);
        public Task<List<Transferencia>> getTransferenciaByUserId(int usuarioiD);
        public Task<bool> Insert(Transferencia transferencia);
    }
}
