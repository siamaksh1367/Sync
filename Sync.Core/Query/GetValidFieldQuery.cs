using Sync.Core.Infrustructure;

namespace Sync.Core.Query
{
    public record GetValidFieldQuery(DateTime Date) : IQuery<IEnumerable<Guid>>;
}
