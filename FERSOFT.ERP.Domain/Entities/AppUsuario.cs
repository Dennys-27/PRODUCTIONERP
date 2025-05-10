using Microsoft.AspNetCore.Identity;

namespace FERSOFT.ERP.Domain.Entities
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
    }
}
