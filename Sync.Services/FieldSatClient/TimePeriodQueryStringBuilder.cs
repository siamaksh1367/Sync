using Sync.Common.Constants;

namespace Sync.Services.FieldSatClient
{
    public class TimePeriodQueryStringBuilder(IConstantManager constantManager) : IQueryStringBuilder
    {
        private readonly IConstantManager _constantManager = constantManager;

        public Dictionary<string, string> SetQueryString(params object[] parameters)
        {
            var startDate = parameters.FirstOrDefault() as DateTime?;
            var endDate = parameters.LastOrDefault() as DateTime?;
            return new Dictionary<string, string> {
                {
                    _constantManager.Since(),
                    startDate?.ToString(_constantManager.QueryFormat())
                },
                {
                    _constantManager.Until(),
                    endDate?.ToString(_constantManager.QueryFormat())
                }
            };
        }
    }
}
