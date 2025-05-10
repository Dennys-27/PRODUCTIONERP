using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Domain.Interfaces;
using FERSOFT.ERP.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Infrastructure.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioRepositorio(
            ApplicationDbContext db,
            UserManager<AppUsuario> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IList<string>> GetRolesAsync(AppUsuario usuario)
        {
            return await _userManager.GetRolesAsync(usuario);
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _db.Set<AppUsuario>().Find(usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
           return _db.Set<AppUsuario>().OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUsuario> LoginAsync(string nombreUsuario, string password)
        {
            var usuario = await _userManager.Users
               .FirstOrDefaultAsync(u => u.UserName.ToLower() == nombreUsuario.ToLower());
            if (usuario == null) return null;
            return await _userManager.CheckPasswordAsync(usuario, password) ? usuario : null;
        }

        public async Task<AppUsuario> RegistroAsync(AppUsuario usuario, string password)
        {
            var result = await _userManager.CreateAsync(usuario, password);
            if (!result.Succeeded) return null;

            // Seed roles if not exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Registrado"));
            }
            await _userManager.AddToRoleAsync(usuario, "Admin");

            return usuario;
        }
    }
}
