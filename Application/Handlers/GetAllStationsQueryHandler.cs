using Application.DTOs;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class GetAllStationsQueryHandler : IRequestHandler<GetAllStationsQuery, IEnumerable<StationDto>>
    {
        private readonly IStationRepository _stationRepository;

        public GetAllStationsQueryHandler(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public async Task<IEnumerable<StationDto>> Handle(GetAllStationsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Station> stations = await _stationRepository.GetAllAsync(cancellationToken);

            return stations.Select(station => new StationDto(station.Id, station.Name, station.Reference));
        }
    }
}
