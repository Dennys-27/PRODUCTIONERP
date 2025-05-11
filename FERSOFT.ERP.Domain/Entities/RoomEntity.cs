using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Domain.Entities
{
    public class RoomEntity : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public short Number { get; set; }
        public ICollection<SeatEntity> Seats { get; set; } = new List<SeatEntity>();
    }
}
