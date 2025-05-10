using AutoMapper;
using FERSOFT.ERP.Application.Interfaces;
using FERSOFT.ERP.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FERSOFT.ERP.API.Controllers.Response;
using FERSOFT.ERP.Infrastructure.Repositorios;
using FERSOFT.ERP.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Net;


namespace FERSOFT.ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        protected RespuestaAPI _respuestaAPI;

        public UsuariosController(IUsuarioService usuarioService, IMapper mapper, IWebHostEnvironment env)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _respuestaAPI = new();
            _env = env;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var listadoUsuarios = _usuarioService.GetUsuarios();
            var listadoUsuariosDto = new List<UsuarioDto>();

            foreach (var list in listadoUsuarios)
            {
                listadoUsuariosDto.Add(_mapper.Map<UsuarioDto>(list));
            }
            return Ok(listadoUsuariosDto);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{usuarioId}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(string usuarioId)
        {
            var itemUsuario = _usuarioService.GetUsuario(usuarioId);
            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);

            return Ok(itemUsuario);
        }

        [AllowAnonymous]
        [HttpPost("Registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromForm] UsuarioRegistroDto usuarioRegistroDto)
        {
            
            if (!ModelState.IsValid)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("Datos de registro inválidos");
                return BadRequest(_respuestaAPI);
            }

            
            if (!_usuarioService.IsUniqueUser(usuarioRegistroDto.NombreUsuario))
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaAPI);
            }

            
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            if (usuarioRegistroDto.Imagen?.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "usuarios");
                Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(usuarioRegistroDto.Imagen.FileName)}";
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await usuarioRegistroDto.Imagen.CopyToAsync(stream);

                usuarioRegistroDto.RutaImagen = $"{baseUrl}/usuarios/{fileName}";
            }
            else
            {
                usuarioRegistroDto.RutaImagen = $"{baseUrl}/images/default-user.png";
            }

            
            var usuarioCreado = await _usuarioService.RegistroAsync(usuarioRegistroDto);
            if (usuarioCreado == null)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.InternalServerError;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("Error en el registro de usuario");
                return StatusCode(StatusCodes.Status500InternalServerError, _respuestaAPI);
            }

            
            _respuestaAPI.StatusCode = HttpStatusCode.Created;
            _respuestaAPI.IsSuccess = true;
            _respuestaAPI.Result = usuarioCreado;  
            return CreatedAtAction(nameof(Registro), new { id = usuarioCreado.Id }, _respuestaAPI);
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuestaLogin = await _usuarioService.LoginAsync(usuarioLoginDto);

            if (respuestaLogin == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de Usuario o Password son Incorrectos");
                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            ///Usamos esto para que nos devuelva el login
            _respuestaAPI.Result = respuestaLogin;
            return Ok(_respuestaAPI);
        }

    }
}
