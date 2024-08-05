using Sync.Services.DTOs;

namespace Sync.Services.FieldSatClient
{
    public interface IFieldSatClient
    {
        public Task<IEnumerable<FieldDto>> GetFieldsAsync();
    }
}
