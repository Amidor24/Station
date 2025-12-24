using MediatR;

namespace Application.Commands
{
    public record DeleteStationCommand(Guid Id) : IRequest<bool>;
}
