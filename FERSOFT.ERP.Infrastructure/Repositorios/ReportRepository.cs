using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Domain.Interfaces;
using FERSOFT.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Infrastructure.Repositorios
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _db;

        public ReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        //a.) Generar el query necesario para obtener las reservas de películas cuyo genero sea terror y con un rango de fechas
        public async Task<IEnumerable<BookingEntity>> GetTerrorBookingsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var bookings = await _db.Bookings
                .Include(b => b.Movie)
                .Where(b =>
                    b.Movie.Genre.ToString().ToLower() == "terror" &&  
                    b.Date >= startDate &&   
                    b.Date <= endDate)
                .ToListAsync();

            return bookings;
        }




        //B) Generar el query necesario para obtener el numero de butacas disponibles y ocupadas por sala en la cartelera del día actual.
        public async Task<IEnumerable<SeatStatusDto>> GetSeatStatusByRoomForTodayAsync()
        {
            var today = DateTime.Today;

            
            var result = await _db.Billboards
                .Where(b => b.Date.Date == today) 
                .Include(b => b.Room)
                .Include(b => b.Seats)
                .GroupBy(b => b.Room.Name)
                .Select(g => new SeatStatusDto
                {
                    RoomName = g.Key,
                    AvailableSeats = g.SelectMany(b => b.Seats).Count(s => !s.IsOccupied),
                    OccupiedSeats = g.SelectMany(b => b.Seats).Count(s => s.IsOccupied)
                })
                .ToListAsync();

            return result;
        }
    }
}
