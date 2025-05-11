using FERSOFT.ERP.API.Controllers.Response;
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
        private readonly IBookingService _bookingService;
        private readonly ISeatService _seatService;

        public BillboardsController(ISeatService seatService, IBillboardService billboardService, IBookingService bookingService)
        {
            _seatService = seatService;
            _bookingService = bookingService;
            _billboardService = billboardService;

        }

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

        [Authorize(Roles = "Admin")]
        [HttpPost("cancelar-reserva")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelBookingAsync(int bookingId)
        {
            var response = new RespuestaAPI();

            try
            {
                await _bookingService.CancelBookingAsync(bookingId);
                response.StatusCode = HttpStatusCode.OK;
                response.Result = new { message = "Booking canceled successfully" };
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
                // Aquí podrías obtener las butacas para todas las carteleras del día de hoy
                var today = DateTime.Now.Date;
                var billboards = await _billboardService.GetAllBillboardsAsync();
                var seatsStatus = new List<SeatStatusDto>();

                foreach (var billboard in billboards)
                {
                    if (billboard.Date.Date == today)
                    {
                        var seats = await _seatService.GetSeatsByRoomAsync(billboard.RoomId);
                        seatsStatus.AddRange(seats.Select(s => new SeatStatusDto
                        {
                            SeatNumber = s.SeatNumber.ToString(),
                            IsAvailable = s.IsAvailable,
                            RoomName = s.RoomName
                        }));
                    }
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Result = seatsStatus;
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
