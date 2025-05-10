using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Infrastructure.Data
{
    public class Menu
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }

        public ICollection<MenuRol> MenuRoles { get; set; }
    }
}
