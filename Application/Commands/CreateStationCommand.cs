using Application.DTOs;
using MediatR;

namespace Application.Commands
{
    public record CreateStationCommand(string Name, string Reference) : IRequest<StationDto> { }
}
