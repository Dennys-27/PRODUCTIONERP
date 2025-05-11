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
        Task<IEnumerable<BookingEntity>> GetTerrorBookingsInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SeatStatusDto>> GetSeatStatusByRoomForTodayAsync();
    }

    public class SeatStatusDto
    {
        public string RoomName { get; set; }
        public int AvailableSeats { get; set; }
        public int OccupiedSeats { get; set; }
    }
}
