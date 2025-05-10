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
        private readonly IRepositoryGeneric<RoomEntity> _roomRepositoty;

        public SeatService(IMapper mapper, IRepositoryGeneric<SeatEntity> seatRepositoty, IRepositoryGeneric<RoomEntity> roomRepositoty)
        {
            _roomRepositoty = roomRepositoty;
            _seatRepositoty = seatRepositoty;
            _mapper = mapper;
        }
        // DeshabilitarAsientoAsync
        public async Task DisableSeatAsync(int seatId)
        {
            
            var seat = await _seatRepositoty.GetByIdAsync(seatId);
            if (seat == null)
                throw new NotFoundException("Seat not found");

            seat.IsAvailable = false;
            _seatRepositoty.Update(seat);
            await _seatRepositoty.SaveAsync();
        }
        // HabilitarAsientoAsync
        public async Task EnableSeatAsync(int seatId)
        {
            var seat = await _seatRepositoty.GetByIdAsync(seatId);
            if (seat == null)
                throw new NotFoundException("Seat not found");

            seat.IsAvailable = true;
            _seatRepositoty.Update(seat);
            await _seatRepositoty.SaveAsync();
        }
        // ObtenerAsientosPorSalaAsync
        public async  Task<IEnumerable<SeatDto>> GetSeatsByRoomAsync(int roomId)
        {
            var seats = await _seatRepositoty.GetAllAsync();

            var roomSeats = seats.Where(s => s.RoomId == roomId).ToList();

            return _mapper.Map<IEnumerable<SeatDto>>(roomSeats);
        }

        
    }
}
