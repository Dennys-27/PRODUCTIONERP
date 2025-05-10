using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.DTOs.Cinema
{
    public class BillboardDto
    {
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public string RoomName { get; set; }
        public string MovieName { get; set; }
    }
}
