using Microsoft.EntityFrameworkCore;
using Sync.DAL.Models;
using Sync.DAL.Repositories.Interfaces;

namespace Sync.DAL.Repositories.Implementations
{
    public class TimePeriodRepository : GenericRepository<TimePeriod>, ITimePeriodRepository
    {
        public TimePeriodRepository(SyncDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TimePeriod>> GetByFieldId(Guid field)
        {
            return await _dbContext.TimePeriods
                    .Include(tp => tp.Fields)
                    .Where(tp => tp.Fields.Any(f => f.Id == field))
                    .ToListAsync();
        }
    }
}
