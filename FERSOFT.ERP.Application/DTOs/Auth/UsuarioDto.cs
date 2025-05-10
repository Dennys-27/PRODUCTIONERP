using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.DTOs.Auth
{
    public class UsuarioDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Nombre { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string? RutaImagen { get; set; }

        public string? RutaLocalImagen { get; set; }
    }
}
