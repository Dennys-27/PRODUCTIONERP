using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.DTOs.Cinema
{
    public class BookingDto
    {
        public int CustomerId { get; set; }
        public int SeatId { get; set; }
        public DateTime Date { get; set; }
        public int MovieId { get; set; }

        public int BillboardId { get; set; } // Asegúrate de tener esta propiedad
    }
}
