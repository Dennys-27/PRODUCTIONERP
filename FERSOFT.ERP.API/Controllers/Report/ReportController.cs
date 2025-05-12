using FERSOFT.ERP.API.Controllers.Response;
using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace FERSOFT.ERP.API.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IBillboardService _billboardService;
        private readonly ISeatService _seatService;
        public ReportController(IBillboardService billboardService, ISeatService seatService)
        {
            _billboardService = billboardService;
            _seatService = seatService;

        }

        //b1.Implementar endpoint para el servicio del punto 3 literal a.
        // Implementar endpoint para obtener las reservas de películas cuyo genero sea terror y en un rango de fechas.
        [Authorize(Roles = "Admin")]
        [HttpGet("report/terror-bookings")]
        public async Task<IActionResult> GetTerrorBookings([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = new RespuestaAPI();
            try
            {
                if (startDate > endDate)
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages.Add("La fecha de inicio no puede ser mayor a la fecha de fin.");
                    return BadRequest(response);
                }

                var result = await _billboardService.GetTerrorBookingsInDateRangeAsync(startDate, endDate);

                response.StatusCode = HttpStatusCode.OK;
                response.Result = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, response);
            }
        }

        //b1. Implementar endpoint para el servicio del punto 3 literal b.
        //Implementar endpoint para obtener el número de butacas disponibles y ocupadas por sala en la cartelera del día actual.
        [Authorize(Roles = "Admin")]
        [HttpGet("estado-butacas-hoy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSeatsStatusTodayAsync()
        {
            var response = new RespuestaAPI();
            try
            {
                var result = await _seatService.GetSeatStatusByRoomForTodayAsync();

                response.StatusCode = HttpStatusCode.OK;
                response.Result = result;
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
