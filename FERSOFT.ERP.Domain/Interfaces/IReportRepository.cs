using FERSOFT.ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Domain.Interfaces
{
    public interface IReportRepository
    {
        // Obtener reservas de películas de terror en un rango de fechas
        Task<IEnumerable<BookingEntity>> GetTerrorBookingsInDateRangeAsync(DateTime startDate, DateTime endDate);

        // Obtener el estado de las butacas por sala para el día de hoy
        Task<IEnumerable<SeatStatusDto>> GetSeatStatusByRoomForTodayAsync();
    }

    public class SeatStatusDto
    {
        public string RoomName { get; set; }
        public int AvailableSeats { get; set; }
        public int OccupiedSeats { get; set; }
    }
}
