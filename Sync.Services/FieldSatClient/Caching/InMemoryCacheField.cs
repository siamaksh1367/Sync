using Microsoft.Extensions.Logging;

namespace Sync.Services.FieldSatClient.Caching
{
    public class InMemoryCacheField : GenericCachManager<Guid>, ICacheManager<Guid>
    {
        public InMemoryCacheField(ILogger<InMemoryCacheField> logger) : base(logger, "Field")
        {

        }
    }
}