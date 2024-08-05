using Sync.DAL.Repositories.Interfaces;

namespace Sync.DAL
{
    public class UnitOfWork(SyncDbContext dbContext, IFieldRepository fieldRepository, IImageRepository imageRepository, IPolygonRepository polygonRepository, ITimePeriodRepository fieldTimeSpanRepository) : IUnitOfWork
    {
        private bool _disposed = false;

        public IFieldRepository FieldRepository { get; } = fieldRepository;
        public IImageRepository ImageRepository { get; } = imageRepository;
        public IPolygonRepository PolygonRepository { get; } = polygonRepository;
        public ITimePeriodRepository TimePeriodRepository { get; } = fieldTimeSpanRepository;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
