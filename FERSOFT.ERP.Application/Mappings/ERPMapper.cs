using AutoMapper;
using FERSOFT.ERP.Application.DTOs.Auth;
using FERSOFT.ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FERSOFT.ERP.Application.Mappings
{
    public class ERPMapper : Profile
    {
        public ERPMapper()
        {
             CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
             CreateMap<AppUsuario, UsuarioDto>().ReverseMap();
            CreateMap<UsuarioRegistroDto, AppUsuario>()
   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NombreUsuario));
            CreateMap<UsuarioDatosDto, UsuarioDto>();
        }
       
        
    }
}
