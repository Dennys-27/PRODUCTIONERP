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

        [Fact]
        public async Task CreateBookingAsync_Should_Throw_When_Seat_Is_Not_Available()
        {
            // Arrange
            var dto = new BookingDto
            {
                CustomerId = 1,
                SeatId = 99,
                BillboardId = 1,
                MovieId = 1,
                Date = DateTime.UtcNow
            };

            // Customer exists
            _customerGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.CustomerId))
                .ReturnsAsync(new CustomerEntity { Id = dto.CustomerId });

            // Seat does NOT exist -> triggers InvalidOperationException
            _seatGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.SeatId))
                .ReturnsAsync((SeatEntity)null);

            // Billboard exists (so we pass that check)
            _billboardGenericRepoMock
                .Setup(r => r.GetByIdAsync(dto.BillboardId))
                .ReturnsAsync(new BillboardEntity { Id = dto.BillboardId });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateBookingAsync(dto));

            Assert.Equal("Seat not available", ex.Message);
        }
    }
}
