using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public record GetStationByIdQuery(Guid Id) : IRequest<StationDto>;
}
