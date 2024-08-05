using Sync.DAL.Models;

namespace Sync.DAL.Repositories.Interfaces
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        Task<IEnumerable<Image>> GetImagesByDate(DateTime date);
        Task<IEnumerable<Image>> GetImagesByFieldId(Guid filedId);
    }
}
