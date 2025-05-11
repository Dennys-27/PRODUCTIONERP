using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.DTOs.Cinema
{
    public class CancelBookingResponseDto
    {
        public string Message { get; set; }
        public IEnumerable<string> AffectedClients { get; set; }
    }
}
