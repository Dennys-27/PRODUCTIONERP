using FERSOFT.ERP.Application.DTOs.Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Interfaces.Cinema
{
    public interface ISeatService
    {
        // Obtiene todos los asientos de una sala usando su ID
        Task<IEnumerable<SeatDto>> GetSeatsByRoomAsync(int roomId);

        // Desactiva un asiento usando su ID
        Task DisableSeatAsync(int seatId);

        // Activa un asiento usando su ID
        Task EnableSeatAsync(int seatId);
    }
}
