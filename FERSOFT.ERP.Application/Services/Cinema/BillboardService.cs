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
        private readonly IMapper _mapper;


        public BillboardService
            (
            IRepositoryGeneric<BillboardEntity> billboardRepository,
            IRepositoryGeneric<MovieEntity> movieRepository,
            IRepositoryGeneric<RoomEntity> roomRepository,
            IMapper mapper
            )
        {
            _billboardRepository = billboardRepository;
            _movieRepository = movieRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        // Método para cancelar un cartel publicitario
        public async Task CancelBillboardAsync(int billboardId)
        {
            var billboard = await _billboardRepository.GetByIdAsync(billboardId);

            if (billboard == null)
                throw new NotFoundException("Billboard not found");

            _billboardRepository.Delete(billboard);

            await _billboardRepository.SaveAsync();
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
                Date = billboardDto.Date
            };

            await _billboardRepository.AddAsync(billboard);
            await _billboardRepository.SaveAsync();

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
    }
}
