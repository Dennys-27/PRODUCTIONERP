using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Domain.Entities
{
    public class BillboardEntity : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public int MovieId { get; set; }
        public MovieEntity Movie { get; set; }

        public int RoomId { get; set; }
        public RoomEntity Room { get; set; }

        public ICollection<SeatEntity> Seats { get; set; }
        public ICollection<BookingEntity> Bookings { get; set; }
        public string? MovieTitle { get; set; }
        public DateTime? FunctionDate { get; set; }
    }
}
