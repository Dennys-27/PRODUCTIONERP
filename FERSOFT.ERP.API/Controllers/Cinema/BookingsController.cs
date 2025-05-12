using FERSOFT.ERP.API.Controllers.Response;
using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Application.Exceptions;
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
        private readonly IBillboardService _billboardService;
        private readonly ISeatService _seatService;
        public BookingsController(IBookingService bookingService, IBillboardService billboardService, ISeatService seatService)
        {
            _bookingService = bookingService;
            _billboardService = billboardService;
            _seatService = seatService;
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

        // Cancelar Reserva
        [Authorize(Roles = "Admin")]
        [HttpPost("cancelar-reserva")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelBookingAsync([FromBody] CancelarReservaButacaDto dto)
        {
            var response = new RespuestaAPI();

            try
            {
                await _bookingService.CancelarReservaYInhabilitarButacaAsync(dto);
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
                var today = DateTime.Now.Date;
                var billboards = await _billboardService.GetAllBillboardsAsync();
                var roomSummaries = new Dictionary<int, SeatStatusDto>();

                foreach (var billboard in billboards.Where(b => b.Date.Date == today))
                {
                    if (!roomSummaries.ContainsKey(billboard.RoomId))
                    {
                        var seats = await _seatService.GetSeatsByRoomAsync(billboard.RoomId);

                        var summary = new SeatStatusDto
                        {
                            RoomName = seats.FirstOrDefault()?.RoomName ?? $"Sala {billboard.RoomId}",
                            AvailableSeats = seats.Count(s => s.IsAvailable),
                            OccupiedSeats = seats.Count(s => !s.IsAvailable)
                        };

                        roomSummaries[billboard.RoomId] = summary;
                    }
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Result = roomSummaries.Values.ToList();
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


        // Crear una nueva reserva
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingDto)
        {
            var response = new RespuestaAPI();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _bookingService.CreateBookingAsync(bookingDto);
                response.StatusCode = HttpStatusCode.Created;
                response.IsSuccess = true;
                response.Result = result;
                // Devuelve 201 con Location al GetBookingById
                return CreatedAtAction(nameof(GetBookingById), new { id = result.Id }, response);
            }
            catch (NotFoundException ex)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessages.Add(ex.Message);
                return NotFound(response);
            }
            catch (InvalidOperationException ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add(ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error inesperado: {ex.Message}");
                return StatusCode(500, response);
            }
        }

        // Obtener una reserva por su ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var response = new RespuestaAPI();
            try
            {
                var booking = await _bookingService.GetBookingsByCustomerAsync(id);
                if (booking == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    response.ErrorMessages.Add("Reserva no encontrada.");
                    return NotFound(response);
                }
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = booking;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add($"Error inesperado: {ex.Message}");
                return StatusCode(500, response);
            }
        }

    }
}
