using Practica_Final.Domain.Entities;
using Practica_Final.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Practica_Final.Infrastructure.Repositories
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly ApplicationDbContext _context;

        public RepositoryUsuario(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> Register(Usuario usuario)
        {
            bool isAdd = true;
            try
            {
                await this._context.Usuarios.AddAsync(usuario);
                await this._context.SaveChangesAsync();
            }
            catch (Exception)
            {
                isAdd = false;
            }
            return isAdd;
        }

        public async Task<bool> IsUsuarioExist(string email)
        {
            bool isExiste = false;
            try
            {
                var usuario = await this._context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email));
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
