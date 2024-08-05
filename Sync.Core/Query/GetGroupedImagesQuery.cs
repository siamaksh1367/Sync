using Sync.Core.Infrustructure;
using Sync.Services.DTOs;

namespace Sync.Core.Query
{
    public record GetGroupedImagesQuery() : IQuery<IEnumerable<IGrouping<DateTime, ImageDto>>>;
}
