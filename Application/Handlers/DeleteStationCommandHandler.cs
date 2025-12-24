using Application.Commands;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers
{
    public class DeleteStationCommandHandler : IRequestHandler<DeleteStationCommand,bool>
    {
        private readonly IStationRepository _stationRepository;

        public DeleteStationCommandHandler(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }
        
        public async Task<bool> Handle(DeleteStationCommand request, CancellationToken cancellationToken)
        {
            var station = await _stationRepository.GetByIdAsync(request.Id, cancellationToken);
        
            if (station == null)
            {
                return false;
            }
            else
            {
                await _stationRepository.DeleteAsync(station.Id, cancellationToken);

                return true;
            }
        }
    }
}
