using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sync.Core.Command;

namespace Sync.Collect
{
    public class CollectImages
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public CollectImages(IMediator mediator, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CollectImages>();
            _mediator = mediator;
        }

        [Function("CollectImages")]
        public async Task RunAsync([TimerTrigger("*/1 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Started CollectImages");
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 02, 01), EndDate = new DateTime(2023, 03, 01) });
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 04, 01), EndDate = new DateTime(2023, 05, 01) });
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 06, 01), EndDate = new DateTime(2023, 07, 01) });
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 09, 01), EndDate = new DateTime(2023, 10, 01) });
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 02, 02), EndDate = new DateTime(2023, 02, 28) });
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 03, 15), EndDate = new DateTime(2023, 05, 15) });
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 03, 02), EndDate = new DateTime(2023, 07, 20) });
            await _mediator.Send(new AddCollectedImagesCommand() { StartDate = new DateTime(2023, 02, 10), EndDate = new DateTime(2023, 09, 20) });
            _logger.LogInformation("Finished CollectImages");
        }
    }
}
