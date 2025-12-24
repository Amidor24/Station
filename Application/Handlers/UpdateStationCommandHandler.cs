using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class UpdateStationCommandHandler : IRequestHandler<UpdateStationCommand, StationDto>
    {
        private readonly IStationRepository _stationRepository;

        public UpdateStationCommandHandler(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public async Task<StationDto> Handle(UpdateStationCommand request, CancellationToken cancellationToken)
        {
            Station station = await _stationRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Station with Id {request.Id} not found.");

            station.Name = request.Name;

            station.Reference = request.Reference;

            await _stationRepository.UpdateAsync(station, cancellationToken);

            return new StationDto(station.Id, station.Name, station.Reference);
        }
    }
}
