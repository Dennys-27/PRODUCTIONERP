using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class UsuarioRegistroDto
{
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string NombreUsuario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El password es obligatorio")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string Role { get; set; }  

    public IFormFile? Imagen { get; set; }  

    public string? RutaImagen { get; set; } 
}
