using Domain.Entities;

namespace Domain.Repositories
{
    public interface IStationRepository
    {
        Task<Station> Add(Station Station);
        Task<IEnumerable<Station>> GetAllAsync(CancellationToken ct = default);
        Task<Station?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task UpdateAsync(Station station, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
