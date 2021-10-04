using Practica_Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Practica_Final.Infrastructure.Repositories
{
    public interface IRepositoryUsuario
    {
        public Task<bool> Register(Usuario usuario);
        public Task<bool> IsUsuarioExist(string email);
    }
}
