using FERSOFT.ERP.API.Controllers.Response;
using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[Route("api/cinema/seats")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    // Obtener asientos por sala
    [Authorize(Roles = "Admin")]
    [HttpGet("room/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSeatsByRoomAsync(int roomId)
    {
        var response = new RespuestaAPI();

        try
        {
            var seats = await _seatService.GetSeatsByRoomAsync(roomId);
            response.StatusCode = HttpStatusCode.OK;
            response.Result = seats;
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

    // Deshabilitar un asiento
    [Authorize(Roles = "Admin")]
    [HttpPost("{seatId}/disable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DisableSeatAsync(int seatId)
    {
        var response = new RespuestaAPI();

        try
        {
            await _seatService.DisableSeatAsync(seatId);
            response.StatusCode = HttpStatusCode.OK;
            response.Result = new { message = "Seat disabled successfully" };
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

    // Habilitar un asiento
    [Authorize(Roles = "Admin")]
    [HttpPost("{seatId}/enable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EnableSeatAsync(int seatId)
    {
        var response = new RespuestaAPI();

        try
        {
            await _seatService.EnableSeatAsync(seatId);
            response.StatusCode = HttpStatusCode.OK;
            response.Result = new { message = "Seat enabled successfully" };
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

    // Crear un asiento
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSeatAsync([FromBody] SeatDto seatDto)
    {
        var response = new RespuestaAPI();
        try
        {
            if (seatDto == null)
            {
                return BadRequest("Invalid seat data.");
            }

            var seat = await _seatService.CreateSeatAsync(seatDto);
            response.Result = seat;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
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

    // Obtener un asiento por ID
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSeatByIdAsync(int id)
    {
        var response = new RespuestaAPI();
        try
        {
            var seat = await _seatService.GetSeatByIdAsync(id);
            if (seat == null)
            {
                return NotFound("Seat not found.");
            }

            response.Result = seat;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
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

    // Actualizar un asiento
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSeatAsync(int id, [FromBody] SeatDto seatDto)
    {
        try
        {
            if (id != seatDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            await _seatService.UpdateSeatAsync(seatDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            var response = new RespuestaAPI
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
            };
            response.ErrorMessages.Add($"Error: {ex.Message}");
            return StatusCode(500, response);
        }
    }

    // Eliminar un asiento
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSeatAsync(int id)
    {
        var response = new RespuestaAPI();
        try
        {
            await _seatService.DeleteSeatAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            response.StatusCode = HttpStatusCode.NotFound;
            response.IsSuccess = false;
            response.ErrorMessages.Add($"Error: {ex.Message}");
            return NotFound(response);
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
