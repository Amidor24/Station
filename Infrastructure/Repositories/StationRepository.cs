using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StationRepository(ApplicationDbContext context) : IStationRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Station> Add(Station Station)
        {
            _context.Stations.Add(Station);

            await _context.SaveChangesAsync();

            return Station;
        }

        public async Task<IEnumerable<Station>> GetAllAsync(CancellationToken ct = default)
        {
            List<Station> stations = await _context.Stations.ToListAsync(ct);

            return stations;
        }

        public async Task<Station?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Stations.FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public Task UpdateAsync(Station station, CancellationToken ct = default)
        {
            _context.Stations.Update(station);
            return _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            Station? station = await GetByIdAsync(id, ct);

            if (station is not null)
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync(ct);
            }
            else
            {
                return;
            }
        }
    }
}
