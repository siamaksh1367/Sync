namespace Sync.Services.MultipleProviders
{
    public interface IDataProvider<T>
    {
        Task<T> ProvideAsync();
    }
}
