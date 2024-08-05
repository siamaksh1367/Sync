using Sync.Services.DTOs;

namespace Sync.Services.FieldSatClient
{
    public interface IImageSatClient
    {
        public Task<IEnumerable<ImageDto>> GetImagesAsync(Guid filedId, Dictionary<string, string> queryParams);
    }
}
