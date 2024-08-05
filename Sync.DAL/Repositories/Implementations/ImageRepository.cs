using Microsoft.EntityFrameworkCore;
using Sync.DAL.Models;
using Sync.DAL.Repositories.Interfaces;

namespace Sync.DAL.Repositories.Implementations
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        public ImageRepository(SyncDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Image>> GetImagesByDate(DateTime date)
        {
            return await _dbContext.Images.Where(x => x.Date == date).ToListAsync();
        }

        public async Task<IEnumerable<Image>> GetImagesByFieldId(Guid filedId)
        {
            return await _dbContext.Images.Where(x => x.FieldId == filedId).ToListAsync();
        }

    }
}
