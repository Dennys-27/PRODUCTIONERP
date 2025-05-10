using AutoMapper;
using FERSOFT.ERP.Application.DTOs.Auth;
using FERSOFT.ERP.Application.Interfaces;
using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
       

        public UsuarioService(
             IUsuarioRepositorio usuarioRepo,
             IMapper mapper, IJwtService jwtService
             )
        {
            _usuarioRepo = usuarioRepo;
            _mapper = mapper;
            _jwtService = jwtService;
            
        }
        public UsuarioDatosDto GetUsuario(string usuarioId)
        {
            var usuario = _usuarioRepo.GetUsuario(usuarioId);
            return _mapper.Map<UsuarioDatosDto>(usuario);
        }

        public ICollection<UsuarioDatosDto> GetUsuarios()
        {
            var usuarios = _usuarioRepo.GetUsuarios();
            return  usuarios.Select(u => _mapper.Map<UsuarioDatosDto>(u)).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            return _usuarioRepo.IsUniqueUser(usuario);
        }

        public async Task<UsuarioLoginRespuestaDto> LoginAsync(UsuarioLoginDto loginDto)
        {
            var usuario = await _usuarioRepo.LoginAsync(loginDto.NombreUsuario, loginDto.Password);
            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto { Token = "", Usuario = null };
            }

            // generar JWT (si no lo haces en repo)
            var roles = await _usuarioRepo.GetRolesAsync(usuario);
            var token = _jwtService.GenerateToken(usuario.UserName, roles);

            return new UsuarioLoginRespuestaDto
            {
                Token = token,
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario),
                Role = roles.FirstOrDefault()
            };

           
        }

        public async Task<UsuarioDatosDto> RegistroAsync(UsuarioRegistroDto registroDto)
        {
            //var usuarioEntidad = _mapper.Map<AppUsuario>(registroDto);
            //entidad.RutaImagen = dto.RutaImagen;
            //var creado = await _usuarioRepo.RegistroAsync(usuarioEntidad, registroDto.Password);
            //return _mapper.Map<UsuarioDatosDto>(creado);
            var entidad = _mapper.Map<AppUsuario>(registroDto);
            // dto.RutaImagen ya contiene la URL a la imagen
            entidad.RutaImagen = registroDto.RutaImagen;

            var creado = await _usuarioRepo.RegistroAsync(entidad, registroDto.Password);
            if (creado == null) return null;

            return _mapper.Map<UsuarioDatosDto>(creado);
        }
    }
}
