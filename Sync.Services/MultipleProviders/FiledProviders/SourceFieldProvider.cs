using Sync.Services.FieldSatClient.Caching;

namespace Sync.Services.MultipleProviders.FiledProviders
{
    public class SourceFieldProvider : IDataProvider<IEnumerable<Guid>>
    {
        private readonly ICacheManager<Guid> _cacheManager;

        public SourceFieldProvider(ICacheManager<Guid> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public Task<IEnumerable<Guid>> ProvideAsync()
        {
            return Task.FromResult(_cacheManager.Get());
        }
    }
}
