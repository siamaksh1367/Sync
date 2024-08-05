using Microsoft.Extensions.Options;
using Sync.Common.Options;

namespace Sync.Services.MultipleProviders.TimeProviders
{
    public class TimePeriodFromParametersProvider(IOptions<Period> option) : IDataProvider<(DateTime? startDate, DateTime? endDate)>
    {
        private readonly IOptions<Period> _option = option;

        public Task<(DateTime? startDate, DateTime? endDate)> ProvideAsync()
        {
            DateTime startDate, endDate;
            var success = false;
            success = DateTime.TryParse(_option.Value.StartDate, out startDate);
            DateTime? startDateOut = success ? startDate : null;
            success = DateTime.TryParse(_option.Value.EndDate, out endDate);
            DateTime? endDateOut = success ? endDate : null;
            return Task.FromResult((startDateOut, endDateOut));
        }
    }
}
