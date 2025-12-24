using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public record GetAllStationsQuery : IRequest<List<StationDto>>;
}
