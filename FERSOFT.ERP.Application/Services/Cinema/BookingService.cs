using AutoMapper;
using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Application.Exceptions;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Services.Cinema
{
    public class BookingService : IBookingService
    {
        private readonly IRepositoryGeneric<BookingEntity> _bookingRepository;
        private readonly IRepositoryGeneric<SeatEntity> _seatRepository;
        private readonly IRepositoryGeneric<CustomerEntity> _customerRepository;
        private readonly IRepositoryGeneric<MovieEntity> _movieRepository;
        private readonly IRepositoryGeneric<BillboardEntity> _billboardRepository;
        private readonly IMapper _mapper;
        public BookingService(
        IRepositoryGeneric<BookingEntity> bookingRepository,
        IRepositoryGeneric<SeatEntity> seatRepository,
        IRepositoryGeneric<CustomerEntity> customerRepository,
        IRepositoryGeneric<MovieEntity> movieRepository,
        IRepositoryGeneric<BillboardEntity> billboardRepository,        
        IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        // Método para cancelar una reserva
        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new NotFoundException("Booking not found");

            var seat = await _seatRepository.GetByIdAsync(booking.SeatId);
            if (seat != null)
            {
                seat.IsAvailable = true;
                _seatRepository.Update(seat);
            }

            _bookingRepository.Delete(booking);
            await _bookingRepository.SaveAsync();
        }

        // Método para crear una reserva
        public async Task<BookingDto> CreateBookingAsync(BookingDto bookingDto)
        {
            var customer = await _customerRepository.GetByIdAsync(bookingDto.CustomerId);
            if (customer == null)
                throw new NotFoundException("Customer not found");

            var seat = await _seatRepository.GetByIdAsync(bookingDto.SeatId);
            if (seat == null || !seat.IsAvailable)
                throw new InvalidOperationException("Seat not available");

            var billboard = await _billboardRepository.GetByIdAsync(bookingDto.BillboardId);
            if (billboard == null)
                throw new NotFoundException("Billboard not found");

            
            var booking = _mapper.Map<BookingEntity>(bookingDto);
            await _bookingRepository.AddAsync(booking);

            seat.IsAvailable = false;
            _seatRepository.Update(seat);

            await _bookingRepository.SaveAsync();

            return _mapper.Map<BookingDto>(booking);
        }

        // Método para obtener las reservas de un cliente
        public async Task<IEnumerable<BookingDto>> GetBookingsByCustomerAsync(int customerId)
        {
            var bookingsList = await _bookingRepository.GetAllAsync();

            var bookings = bookingsList
                .Where(b => b.CustomerId == customerId)
                .ToList();

           
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }


    }
}
