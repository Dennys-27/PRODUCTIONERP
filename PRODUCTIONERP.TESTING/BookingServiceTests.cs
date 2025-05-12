using System;
using System.Threading.Tasks;
using AutoMapper;
using FERSOFT.ERP.Application.DTOs.Cinema;
using FERSOFT.ERP.Application.Exceptions;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using FERSOFT.ERP.Application.Mappings;
using FERSOFT.ERP.Application.Services.Cinema;
using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Domain.Interfaces;
using Moq;
using Xunit;

namespace PRODUCTIONERP.TESTING
{
    public class BookingServiceTests
    {
        private readonly Mock<IRepositoryGeneric<BookingEntity>> _bookingGenericRepoMock;
        private readonly Mock<IRepositoryGeneric<SeatEntity>> _seatGenericRepoMock;
        private readonly Mock<IRepositoryGeneric<CustomerEntity>> _customerGenericRepoMock;
        private readonly Mock<IRepositoryGeneric<MovieEntity>> _movieGenericRepoMock;
        private readonly Mock<IRepositoryGeneric<BillboardEntity>> _billboardGenericRepoMock;
        private readonly Mock<IBookingRepository> _bookingRepoMock;
        private readonly IMapper _mapper;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            // Mocks
            _bookingGenericRepoMock = new Mock<IRepositoryGeneric<BookingEntity>>();
            _seatGenericRepoMock = new Mock<IRepositoryGeneric<SeatEntity>>();
            _customerGenericRepoMock = new Mock<IRepositoryGeneric<CustomerEntity>>();
            _movieGenericRepoMock = new Mock<IRepositoryGeneric<MovieEntity>>();
            _billboardGenericRepoMock = new Mock<IRepositoryGeneric<BillboardEntity>>();
            _bookingRepoMock = new Mock<IBookingRepository>();

            // AutoMapper setup (assume you have a profile named ERPMapper)
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ERPMapper>());
            _mapper = config.CreateMapper();

            // Service under test
            _service = new BookingService(
                _bookingGenericRepoMock.Object,
                _seatGenericRepoMock.Object,
                _customerGenericRepoMock.Object,
                _movieGenericRepoMock.Object,
                _billboardGenericRepoMock.Object,
                _mapper,
                _bookingRepoMock.Object
            );
        }

        // Prueba unitaria: CreateBookingAsync lanza InvalidOperationException si el asiento no está disponible
        [Fact]
        public async Task CreateBookingAsync_Should_Throw_When_Seat_Is_Not_Available()
        {
            
            var dto = new BookingDto
            {
                CustomerId = 1,
                SeatId = 99,
                BillboardId = 1,
                MovieId = 1,
                Date = DateTime.UtcNow
            };

            
            _customerGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.CustomerId))
                .ReturnsAsync(new CustomerEntity { Id = dto.CustomerId });

            
            _seatGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.SeatId))
                .ReturnsAsync((SeatEntity)null);

            
            _billboardGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.BillboardId))
                .ReturnsAsync(new BillboardEntity { Id = dto.BillboardId });

            
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateBookingAsync(dto));

            Assert.Equal("Seat not available", ex.Message);
        }



        // Prueba unitaria: CancelarReservaYInhabilitarButacaAsync actualiza asiento y reserva dentro de la transacción
        [Fact]
        public async Task CancelarReservaYInhabilitarButacaAsync_Should_UpdateSeatAndCancelBooking_InTransaction()
        {
            
            var dto = new CancelarReservaButacaDto
            {
                ButacaId = 10,
                ReservaId = 20
            };

            var seat = new SeatEntity { Id = dto.ButacaId, IsAvailable = true };
            var booking = new BookingEntity { Id = dto.ReservaId, Status = true };

            _seatGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.ButacaId))
                .ReturnsAsync(seat);

            _bookingGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.ReservaId))
                .ReturnsAsync(booking);

            
            _bookingRepoMock
                .Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async op => { await op(); });

            
            await _service.CancelarReservaYInhabilitarButacaAsync(dto);

            
            Assert.False(seat.IsAvailable);

            
            _seatGenericRepoMock.Verify(r => r.UpdateAsync(It.Is<SeatEntity>(s => s == seat)), Times.Once);
            _bookingGenericRepoMock.Verify(r => r.UpdateAsync(It.Is<BookingEntity>(b => b == booking && b.Status == false)), Times.Once);

           
            _bookingRepoMock.Verify(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()), Times.Once);
        }
    }

}
