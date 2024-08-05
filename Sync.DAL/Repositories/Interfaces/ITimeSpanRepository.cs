using Sync.DAL.Models;

namespace Sync.DAL.Repositories.Interfaces
{
    public interface ITimePeriodRepository : IGenericRepository<TimePeriod>
    {
        Task<IEnumerable<TimePeriod>> GetByFieldId(Guid field);
    }
}
