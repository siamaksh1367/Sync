using MediatR;
using Sync.Core.Infrustructure;

namespace Sync.Core.Command
{
    public class AddCollectedImagesCommand : ICommand<Unit>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<Guid>? Fields { get; set; }
    }
}
