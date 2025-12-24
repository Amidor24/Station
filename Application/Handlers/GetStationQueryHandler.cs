using Application.DTOs;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class GetStationQueryHandler : IRequestHandler<GetStationByIdQuery, StationDto>
    {
        private readonly IStationRepository _stationRepository;

        public GetStationQueryHandler(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public async Task<StationDto> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
        {
            Station? station = await _stationRepository.GetByIdAsync(request.Id, cancellationToken);

            return station == null ? null! : new StationDto(station.Id, station.Name, station.Reference);
        }
    }
}
