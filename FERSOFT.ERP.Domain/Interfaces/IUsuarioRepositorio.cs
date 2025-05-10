using FERSOFT.ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Domain.Interfaces
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario> GetUsuarios();
        AppUsuario GetUsuario(string usuarioId);
        bool IsUniqueUser(string usuario);
        Task<AppUsuario> LoginAsync(string nombreUsuario, string password);
        Task<AppUsuario> RegistroAsync(AppUsuario usuario, string password);
        Task<IList<string>> GetRolesAsync(AppUsuario usuario);

    }
}
