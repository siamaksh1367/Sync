using MediatR;

namespace Sync.Core.Infrustructure
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
