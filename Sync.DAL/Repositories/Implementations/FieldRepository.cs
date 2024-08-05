using Microsoft.EntityFrameworkCore;
using Sync.DAL.Models;
using Sync.DAL.Repositories.Interfaces;

namespace Sync.DAL.Repositories.Implementations
{
    public class FieldRepository : GenericRepository<Field>, IFieldRepository
    {
        public FieldRepository(SyncDbContext context) : base(context)
        {
        }

        public async Task<Field> GetById(Guid fieldId)
        {
            return await _dbContext.Fields
                           .Where(x => x.Id == fieldId).FirstOrDefaultAsync();
        }
    }
}
