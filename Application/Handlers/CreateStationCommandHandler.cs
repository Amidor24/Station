using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class CreateStationCommandHandler : IRequestHandler<CreateStationCommand, StationDto>
    {
        private readonly IStationRepository _stationRepository;

        public CreateStationCommandHandler(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public async Task<StationDto> Handle(CreateStationCommand request, CancellationToken cancellationToken)
        {
            Station station = new()
            {
                Name = request.Name,
                Reference = request.Reference
            };

            Station createdStation = await _stationRepository.Add(station);

            StationDto stationDto = new(
                    createdStation.Id,
                    createdStation.Name,
                    createdStation.Reference
                );

            return stationDto;
        }
    }
}
