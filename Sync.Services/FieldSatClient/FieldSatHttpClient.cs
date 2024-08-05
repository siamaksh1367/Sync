using Microsoft.Extensions.Options;
using Sync.Common.Options;
using Sync.Services.DTOs;
using Sync.Services.DTOs.Converter;
using Sync.Services.DTOs.FieldSatResponse;
using System.Text.Json;

namespace Sync.Services.FieldSatClient
{
    public class FieldSatHttpClient(IHttpClientFactory fieldSatClient, IOptions<HttpEndPoints> options) : IFieldSatClient
    {
        private readonly IHttpClientFactory _fieldSatClient = fieldSatClient;
        private readonly IOptions<HttpEndPoints> _options = options;

        public async Task<IEnumerable<FieldDto>> GetFieldsAsync()
        {
            var client = _fieldSatClient.CreateClient();
            HttpResponseMessage response = await client.GetAsync(_options.Value.FeilSatFieldEndPoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                Converters = { new FieldConverter() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<FieldsResponse>(responseBody, options);
            return result.Fields;
        }
    }
}
