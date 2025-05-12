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



            CreateMap<BookingDto, BookingEntity>()
            // Si en BookingEntity hay propiedades que no existen en el DTO, ignóralas:
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Seat, opt => opt.Ignore())
            .ForMember(dest => dest.Billboard, opt => opt.Ignore())
            .ForMember(dest => dest.Movie, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore());

            CreateMap<BookingEntity, BookingDto>()
                // Si BookingEntity tiene más campos que no están en el DTO, ignóralos:
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.SeatId, opt => opt.MapFrom(src => src.SeatId))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.BillboardId, opt => opt.MapFrom(src => src.BillboardId));
        }
       
        
    }
}
