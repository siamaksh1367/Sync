using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sync.Core.Command;

namespace Sync.Collect
{
    public class CollectFields(ILoggerFactory loggerFactory, IMediator mediator)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CollectFields>();
        private readonly IMediator _mediator = mediator;

        [Function("CollectFields")]
        public async Task RunAsync([TimerTrigger("*/1 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Started CollectFields");
            await _mediator.Send(new AddCollectedFieldsCommand());
            _logger.LogInformation("Finished CollectFields");
        }
    }
}
