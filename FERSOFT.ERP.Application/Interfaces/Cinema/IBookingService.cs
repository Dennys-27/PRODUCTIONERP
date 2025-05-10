using FERSOFT.ERP.Application.DTOs.Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Interfaces.Cinema
{
    public interface IBookingService
    {
        // Crea una nueva reserva con los datos que se envían
        Task<BookingDto> CreateBookingAsync(BookingDto bookingDto);

        // Devuelve todas las reservas hechas por un cliente usando su ID
        Task<IEnumerable<BookingDto>> GetBookingsByCustomerAsync(int customerId);

        // Cancela una reserva usando su ID
        Task CancelBookingAsync(int bookingId);
    }
}
