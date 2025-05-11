using AutoMapper;
using FERSOFT.ERP.Application.DTOs.Auth;
using FERSOFT.ERP.Application.DTOs.Cinema;
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


            //Cinema
            

            CreateMap<BillboardEntity, BillboardDto>()
            .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Name))  
            .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name));

            CreateMap<BookingEntity, BookingDto>()
            .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Billboard.MovieId));


            // Mapeo de SeatEntity a SeatDto
            CreateMap<SeatEntity, SeatDto>()
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Number))     // Mapeo del número del asiento
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.RowNumber))   // Mapeo de la fila
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name))    // Mapeo del nombre de la sala
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable)) // Mapeo de la disponibilidad
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId));        // Mapeo del RoomId

            // Mapeo de SeatDto a SeatEntity (por ejemplo, para crear o actualizar un asiento)
            CreateMap<SeatDto, SeatEntity>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.SeatNumber))   // Mapeo del número del asiento
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.RowNumber))  // Mapeo de la fila
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))  // Mapeo de la disponibilidad
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId));       // Mapeo del RoomId
        }
       
        
    }
}
