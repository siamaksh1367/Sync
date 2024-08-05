using Microsoft.Extensions.Logging;

namespace Sync.Services.FieldSatClient.Caching
{
    public class InMemoryCacheImage : GenericCachManager<ImageCache>, ICacheManager<ImageCache>
    {
        public InMemoryCacheImage(ILogger<InMemoryCacheImage> logger) : base(logger, "Image")
        {

        }
    }
}