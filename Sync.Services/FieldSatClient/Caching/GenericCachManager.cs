using Microsoft.Extensions.Logging;

namespace Sync.Services.FieldSatClient.Caching
{
    public abstract class GenericCachManager<T>(ILogger<GenericCachManager<T>> logger, string name) : ICacheManager<T> where T : notnull
    {
        private readonly ILogger<GenericCachManager<T>> _logger = logger;
        private readonly string _name = name;
        private readonly List<T> _list = new List<T>();

        public bool Add(T t)
        {
            if (!RecordExists(t))
            {
                _list.Add(t);
                _logger.LogInformation(string.Format("{0} with id {1} is Added", _name, t));
                return true;
            }
            else
            {
                _logger.LogInformation(string.Format("{0} with id {1} already exists", _name, t));
                return false;
            }
        }

        public bool Delete(T t)
        {
            if (!RecordExists(t))
            {
                _list.Remove(t);
                _logger.LogInformation(string.Format("{0} with id {1} is Deleted", _name, t));
                return true;
            }
            else
            {
                _logger.LogInformation(string.Format("{0} with id {1} does not exists", _name, t));
                return false;
            }
        }

        public bool Exists(T t)
        {
            return RecordExists(t);
        }

        public IEnumerable<T> Get()
        {
            return _list;
        }

        public bool IsEmpty()
        {
            return _list.Count() == 0 || _list is null;
        }

        private bool RecordExists(T t)
        {
            return _list.Exists(x => x.Equals(t));
        }
    }
}