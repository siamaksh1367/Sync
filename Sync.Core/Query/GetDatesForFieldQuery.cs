using Sync.Core.Infrustructure;

namespace Sync.Core.Query
{
    public record GetDatesForFieldQuery(Guid FiledId) : IQuery<IEnumerable<string>>;
}