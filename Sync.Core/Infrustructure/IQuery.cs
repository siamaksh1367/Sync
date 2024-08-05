using MediatR;

namespace Sync.Core.Infrustructure
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
