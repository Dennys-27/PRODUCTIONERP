using FERSOFT.ERP.Application.DTOs.Auth;
using FERSOFT.ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Interfaces
{
    public interface IUsuarioService
    {
        ICollection<UsuarioDatosDto> GetUsuarios();
        UsuarioDatosDto GetUsuario(string usuarioId);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> LoginAsync(UsuarioLoginDto loginDto);
        Task<UsuarioDatosDto> RegistroAsync(UsuarioRegistroDto registroDto);
    }
}
