using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.DTOs.Cinema
{
    public class MovieDto
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public short AllowedAge { get; set; }
        public short LengthMinutes { get; set; }
    }
}
