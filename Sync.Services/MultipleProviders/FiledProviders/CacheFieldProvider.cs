using Sync.Services.FieldSatClient;

namespace Sync.Services.MultipleProviders.FiledProviders
{
    public class CacheFieldProvider : IDataProvider<IEnumerable<Guid>>
    {
        private readonly IFieldSatClient _fieldSatClient;

        public CacheFieldProvider(IFieldSatClient fieldSatClient)
        {
            _fieldSatClient = fieldSatClient;
        }

        public async Task<IEnumerable<Guid>> ProvideAsync()
        {
            return (await _fieldSatClient.GetFieldsAsync()).Select(x => x.Id);
        }
    }
}
