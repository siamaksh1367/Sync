namespace Sync.Services.FieldSatClient.Caching
{
    public interface ICacheManager<T> where T : notnull
    {
        bool Add(T t);
        bool Delete(T t);
        bool Exists(T t);
        bool IsEmpty();
        IEnumerable<T> Get();
    }
}