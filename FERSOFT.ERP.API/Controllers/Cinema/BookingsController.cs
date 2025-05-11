using FERSOFT.ERP.API.Controllers.Response;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using FERSOFT.ERP.Application.Services.Cinema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FERSOFT.ERP.API.Controllers.Cinema
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("reservas/movie/{movieId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookingsByMovieAsync(int movieId)
        {
            var response = new RespuestaAPI();

            try
            {
                var bookings = await _bookingService.GetBookingsByCustomerAsync(movieId);

                if (!bookings.Any())
                {
                    return NotFound(new { message = "No bookings found for this movie" });
                }

                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = bookings;
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
