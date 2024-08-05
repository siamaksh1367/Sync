using Sync.DAL.Repositories.Interfaces;

namespace Sync.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IFieldRepository FieldRepository { get; }
        IImageRepository ImageRepository { get; }
        IPolygonRepository PolygonRepository { get; }
        ITimePeriodRepository TimePeriodRepository { get; }

        Task<int> SaveAsync();

    }
}
