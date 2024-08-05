namespace Sync.Services.FieldSatClient
{
    public interface IQueryStringBuilder
    {
        Dictionary<string, string> SetQueryString(params object[] parameters);
    }
}
