using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.DTOs.Cinema
{
    public class SeatDto
    {
        public int RoomId { get; set; }
        public bool IsAvailable { get; set; }
        public int SeatNumber { get; set; }
        public string RoomName { get; set; }
    }
}
