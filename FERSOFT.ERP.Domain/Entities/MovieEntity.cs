using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Domain.Entities
{
    public enum MovieGenreEnum
    {
        ACTION, ADVENTURE, COMEDY, DRAMA, FANTASY, HORROR, MUSICALS, MYSTERY, ROMANCE, SCIENCE_FICTION, SPORTS, THRILLER, WESTERN
    }

    public class MovieEntity : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public MovieGenreEnum Genre { get; set; }

        [Required]
        public short AllowedAge { get; set; }

        [Required]
        public short LengthMinutes { get; set; }
    }
}
