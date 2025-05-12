using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Interfaces.Cinema
{
    public interface ISeatService
    {
        // Create
        Task<SeatDto> CreateSeatAsync(SeatDto seatDto);

        // Read
        Task<IEnumerable<SeatDto>> GetSeatsByRoomAsync(int roomId);
        Task<SeatDto> GetSeatByIdAsync(int seatId);

        // Update
        Task UpdateSeatAsync(SeatDto seatDto);

        // Delete (o Inhabilitar)
      
        Task DeleteSeatAsync(int seatId); // borrado real


        Task<IEnumerable<SeatStatusDto>> GetSeatStatusByRoomForTodayAsync();
    }
}
