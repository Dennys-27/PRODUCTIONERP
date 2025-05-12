using FERSOFT.ERP.API.Controllers.Response;
using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FERSOFT.ERP.API.Controllers.Cinema
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillboardsController : ControllerBase
    {
        private readonly IBillboardService _billboardService;
       

        public BillboardsController( IBillboardService billboardService)
        {
            
            _billboardService = billboardService;

        }

        // ===============================
        // Cancelar Cartelera (Funcionalidad Importante)
        // ===============================

        // A) Cancelar cartelera y todas las reservas de la sala, habilitar butacas e imprimir clientes afectados
        // B) Si la fecha de la función es anterior a hoy, lanzar excepción personalizada:
        //    "No se puede cancelar funciones de la cartelera con fecha anterior a la actual"
        [Authorize(Roles = "Admin")]
        [HttpPost("cancelar-cartelera")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelBillboardAsync(int billboardId)
        {
            var response = new RespuestaAPI();

            try
            {
                await _billboardService.CancelBillboardAsync(billboardId);
                response.StatusCode = HttpStatusCode.OK;
                response.Result = new { message = "Billboard canceled successfully" };
                return Ok(response);
            }
            catch (Exception ex)
            {

                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, response);
            }

          
        }


        

        // Crear una nueva cartelera
        [Authorize(Roles = "Admin")]
        [HttpPost("crear")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBillboardAsync([FromBody] BillboardDto billboardDto)
        {
            var response = new RespuestaAPI();
            try
            {
                var createdBillboard = await _billboardService.CreateBillboardAsync(billboardDto);
                response.StatusCode = HttpStatusCode.Created;
                response.Result = createdBillboard;
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, response);
            }
        }

        // Obtener todas las carteleras
        [HttpGet("todas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBillboardsAsync()
        {
            var response = new RespuestaAPI();
            try
            {
                var billboards = await _billboardService.GetAllBillboardsAsync();
                response.StatusCode = HttpStatusCode.OK;
                response.Result = billboards;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, response);
            }
        }

        // Obtener una cartelera por ID
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBillboardByIdAsync(int id)
        {
            var response = new RespuestaAPI();
            try
            {
                var billboard = await _billboardService.GetBillboardByIdAsync(id);
                if (billboard == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    response.ErrorMessages.Add("Billboard not found.");
                    return NotFound(response);
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Result = billboard;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, response);
            }
        }

        // Actualizar una cartelera
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBillboardAsync(int id, [FromBody] BillboardDto billboardDto)
        {
            var response = new RespuestaAPI();
            try
            {
                if (id != billboardDto.Id)
                {
                    return BadRequest("ID mismatch.");
                }

                await _billboardService.UpdateBillboardAsync(billboardDto);
                response.StatusCode = HttpStatusCode.OK;
                response.Result = new { message = "Billboard updated successfully" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, response);
            }
        }

        // Eliminar una cartelera
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBillboardAsync(int id)
        {
            var response = new RespuestaAPI();
            try
            {
                await _billboardService.DeleteBillboardAsync(id);

                // Luego simplemente asumes que si no lanzó excepción, fue exitoso
                response.StatusCode = HttpStatusCode.OK;
                response.Result = new { message = "Billboard deleted successfully" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, response);
            }
        }
    }
}
