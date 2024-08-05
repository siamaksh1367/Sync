using Sync.DAL.Models;
using Sync.DAL.Repositories.Interfaces;

namespace Sync.DAL.Repositories.Implementations
{
    public class PolygonRepository : GenericRepository<Polygon>, IPolygonRepository
    {
        public PolygonRepository(SyncDbContext context) : base(context)
        {
        }
    }
}
