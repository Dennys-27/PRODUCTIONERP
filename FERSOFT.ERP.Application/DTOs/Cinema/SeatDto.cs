using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.DTOs.Cinema
{
    public class SeatDto
    {
        public int Id { get; set; }               // <-- Necesario para actualizar
        public int RoomId { get; set; }
        public bool IsAvailable { get; set; }
        public int SeatNumber { get; set; }       // Equivale a Entity.Number
        public int RowNumber { get; set; }        // Equivale a Entity.RowNumber
        public string RoomName { get; set; }


    }
}
