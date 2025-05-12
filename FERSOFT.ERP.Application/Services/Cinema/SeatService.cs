using AutoMapper;
using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Domain.Interfaces;
using FERSOFT.ERP.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Services.Cinema
{
    public class SeatService : ISeatService
    {
        private readonly IRepositoryGeneric<SeatEntity> _seatRepositoty;
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;

        public SeatService(IMapper mapper, IRepositoryGeneric<SeatEntity> seatRepositoty, IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
            _seatRepositoty = seatRepositoty;
            _mapper = mapper;
        }

        public async Task<SeatDto> CreateSeatAsync(SeatDto seatDto)
        {
            try
            {
                Console.WriteLine($"Seat Number: {seatDto.SeatNumber}, Row Number: {seatDto.RowNumber}, RoomId: {seatDto.RoomId}");
                var seatEntity = _mapper.Map<SeatEntity>(seatDto);
                await _seatRepositoty.AddAsync(seatEntity);

                // Verificar si el asiento se guardó correctamente
                Console.WriteLine($"Created Seat Id: {seatEntity.Id}");
                return _mapper.Map<SeatDto>(seatEntity);
            }
            catch (AutoMapperMappingException ex)
            {
                
                Console.WriteLine($"Error en AutoMapper: {ex.Message}");

                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Excepción interna: {ex.InnerException.Message}");
                }

                
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                
                throw;  
            }

        }

        public async Task DeleteSeatAsync(int seatId)
        {
            var seat = await _seatRepositoty.GetByIdAsync(seatId);
            if (seat == null)
                throw new NotFoundException("Seat not found.");

            await _seatRepositoty.DeleteAsync(seat);
        }


        public async Task<SeatDto> GetSeatByIdAsync(int seatId)
        {
            var seat = await _seatRepositoty.GetByIdAsync(seatId);
            if (seat == null)
                throw new NotFoundException("Seat not found.");

            return _mapper.Map<SeatDto>(seat);
        }

        // ObtenerAsientosPorSalaAsync
        public async  Task<IEnumerable<SeatDto>> GetSeatsByRoomAsync(int roomId)
        {
            var seats = await _seatRepositoty.FindAsync(s => s.RoomId == roomId);
            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }

        public async Task UpdateSeatAsync(SeatDto seatDto)
        {
            var seat = await _seatRepositoty.GetByIdAsync(seatDto.Id);
            if (seat == null)
                throw new NotFoundException("Seat not found.");

            seat.Number = (short)seatDto.SeatNumber;
            seat.RowNumber = (short)seatDto.RowNumber;
            seat.IsAvailable = seatDto.IsAvailable;
            seat.RoomId = seatDto.RoomId;

            await _seatRepositoty.UpdateAsync(seat);
        }

        public async Task<IEnumerable<SeatStatusDto>> GetSeatStatusByRoomForTodayAsync()
        {
            return await _reportRepository.GetSeatStatusByRoomForTodayAsync();
        }
    }
}
