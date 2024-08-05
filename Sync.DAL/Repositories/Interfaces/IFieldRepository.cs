using Sync.DAL.Models;

namespace Sync.DAL.Repositories.Interfaces
{
    public interface IFieldRepository : IGenericRepository<Field>
    {
        Task<Field> GetById(Guid field);
    }
}
