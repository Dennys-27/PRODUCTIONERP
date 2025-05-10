using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Infrastructure.Data
{
    public class MenuRol
    {
        
        public int Id { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
    }
}
