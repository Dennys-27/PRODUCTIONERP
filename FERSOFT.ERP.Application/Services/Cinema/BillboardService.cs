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
    public class BillboardService : IBillboardService
    {
        private readonly IRepositoryGeneric<BillboardEntity> _billboardRepository;
        private readonly IRepositoryGeneric<MovieEntity> _movieRepository;
        private readonly IRepositoryGeneric<RoomEntity> _roomRepository;
        private readonly IRepositoryGeneric<SeatEntity> _seatRepository;
        private readonly IBillboardRepository _billboardRepo;
        private readonly IRepositoryGeneric<BookingEntity> _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;

        public BillboardService
            (
            IRepositoryGeneric<BillboardEntity> billboardRepository,
            IRepositoryGeneric<MovieEntity> movieRepository,
            IRepositoryGeneric<RoomEntity> roomRepository,
            IBillboardRepository billboardRepo,
            IMapper mapper,
            IReportRepository reportRepository
            )
        {
            _billboardRepository = billboardRepository;
            _movieRepository = movieRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _billboardRepo = billboardRepo;
            _reportRepository = reportRepository;
        }

        // Método para cancelar un cartel publicitario Metodo Importante
        // A ) Implementar el método con transaccionalidad para inhabilitar la butaca y cancelar la reserva.
        public async Task CancelBillboardAsync(int billboardId)
        {
            await _billboardRepo.ExecuteInTransactionAsync(async () =>
            {
                var billboard = await _billboardRepo.GetBillboardWithDetailsAsync(billboardId);

                if (billboard == null)
                    throw new NotFoundException("Cartelera no encontrada");

                if (billboard.Date.Date < DateTime.Today)
                    throw new BusinessException("No se puede cancelar funciones de la cartelera con fecha anterior a la actual");

                var affectedClients = new List<string>();

                // Cancelar reservas y registrar clientes
                foreach (var booking in billboard.Bookings)
                {
                    booking.Status = false;
                    affectedClients.Add(booking.Customer?.Name ?? $"ClienteId: {booking.CustomerId}");
                    await _bookingRepository.UpdateAsync(booking);
                }

                // Habilitar todas las butacas
                foreach (var seat in billboard.Room.Seats)
                {
                    seat.IsAvailable = true;
                    await _seatRepository.UpdateAsync(seat);
                }

                // Opción 1: Cambiar estado
                billboard.Status = false;
                await _billboardRepository.UpdateAsync(billboard);

              

                Console.WriteLine("Clientes afectados por la cancelación:");
                foreach (var client in affectedClients.Distinct())
                {
                    Console.WriteLine($"- {client}");
                }
            });
        }

        // Método para crear un cartel publicitario
        public async Task<BillboardDto> CreateBillboardAsync(BillboardDto billboardDto)
        {
            var movie = await _movieRepository.GetByIdAsync(billboardDto.MovieId);
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }

            var room = await _roomRepository.GetByIdAsync(billboardDto.RoomId);
            if (room == null)
            {
                throw new NotFoundException("Room not found");
            }

            var billboard = new BillboardEntity
            {
                MovieId = billboardDto.MovieId,
                RoomId = billboardDto.RoomId,
                Date = billboardDto.Date,
                FunctionDate = billboardDto.FunctionDate,
                MovieTitle = billboardDto.MovieName
            };

            await _billboardRepository.AddAsync(billboard);
            //await _billboardRepository.SaveAsync();

            var billboardDtoResult = _mapper.Map<BillboardDto>(billboard);

            
            billboardDtoResult.MovieName = movie.Name;
            billboardDtoResult.RoomName = room.Name;
            
            return billboardDtoResult;
        }

       

        // Método para obtener todos los carteles publicitarios
        public async Task<IEnumerable<BillboardDto>> GetAllBillboardsAsync()
        {

            var billboards = await _billboardRepository.GetAllAsync();

            // Mapeo usando AutoMapper
            return _mapper.Map<IEnumerable<BillboardDto>>(billboards);
        }

        public async Task<BillboardDto> GetBillboardByIdAsync(int id)
        {

            var billboard = await _billboardRepository.GetByIdAsync(id);
            if (billboard == null)
                throw new NotFoundException("Billboard not found.");

            return _mapper.Map<BillboardDto>(billboard);
        }

        public async Task UpdateBillboardAsync(BillboardDto billboardDto)
        {
            var billboard = await _billboardRepository.GetByIdAsync(billboardDto.Id);
            if (billboard == null)
                throw new NotFoundException("Billboard not found.");

            billboard.MovieTitle = billboardDto.MovieTitle;
            billboard.FunctionDate = billboardDto.FunctionDate;
            billboard.RoomId = billboardDto.RoomId;

            await _billboardRepository.UpdateAsync(billboard);
        }

        public async Task DeleteBillboardAsync(int id)
        {
            var billboard = await _billboardRepository.GetByIdAsync(id);
            if (billboard == null)
                throw new NotFoundException("Billboard not found.");

            await _billboardRepository.UpdateAsync(billboard);
            await _billboardRepository.SaveAsync(); // si aplica
        }

        public async Task<IEnumerable<BookingDto>> GetTerrorBookingsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var bookings = await _reportRepository.GetTerrorBookingsInDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
    }
}
