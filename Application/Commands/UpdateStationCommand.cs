using Application.DTOs;
using MediatR;

namespace Application.Commands
{
    public record UpdateStationCommand(Guid Id,string Name, string Reference) : IRequest<StationDto> { }
}