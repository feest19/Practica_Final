using Microsoft.EntityFrameworkCore;
using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica_Final.Infrastructure.Repositories
{
   public class RepositoryTransferencia:IRepositoryTransferencia
    {
        private readonly Contexts.ApplicationDbContext _context;
        //TODO: Hacer la inyeccion de dependencia. Refactorizar el codigo
        public RepositoryTransferencia(Contexts.ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Transferencia>> getTransferenciaByCuentaId(int cuentaID) {
            List<Transferencia> lTransferencias;
                var query = from t in _context.Transferencias
                            where t.CuentaBancariaDestinatarioId == cuentaID || t.CuentaBancariaRemitentetarioId == cuentaID
                            orderby t.Fecha descending
                            select t;
                lTransferencias = await query.ToListAsync();
            return lTransferencias;
        }

        public async Task<List<Transferencia>> getTransferenciaByUserId(int usuarioiD)
        {
            List<Transferencia> lTransferencias;
            var query = from t in _context.Transferencias
                        join c in _context.CuentasBancarias on t.CuentaBancariaRemitentetarioId equals c.Id 
                        where c.UsuarioId == usuarioiD
                        orderby t.Fecha descending
                        select t;
            lTransferencias = await query.ToListAsync();
            return lTransferencias;
        }

        public async Task<bool> Insert(Transferencia transferencia)
        {
            await this._context.Transferencias.AddAsync(transferencia);
            int isSuccess = await _context.SaveChangesAsync();
            return isSuccess > 0;
        }
    }
}
