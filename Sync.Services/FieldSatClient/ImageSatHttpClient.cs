using Microsoft.Extensions.Options;
using Sync.Common.Options;
using Sync.Services.DTOs;
using Sync.Services.DTOs.FieldSatResponse;
using System.Text.Json;
using System.Web;

namespace Sync.Services.FieldSatClient
{
    public class ImageSatHttpClient(IHttpClientFactory imageSatClient, IOptions<HttpEndPoints> options) : IImageSatClient
    {
        private readonly IHttpClientFactory _imageSatClient = imageSatClient;
        private readonly IOptions<HttpEndPoints> _options = options;

        public async Task<IEnumerable<ImageDto>> GetImagesAsync(Guid filedId, Dictionary<string, string> queryParams)
        {
            var client = _imageSatClient.CreateClient();
            var query = BuildQueryString(queryParams);
            HttpResponseMessage response = await client.GetAsync($"{_options.Value.FeilSatImagesEndPoint.ToString().Replace("{fieldId}", filedId.ToString())}?{query}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<ImagesResponse>(responseBody, options);
            return result.Images;
        }

        private string BuildQueryString(Dictionary<string, string> queryParams)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }
            return query.ToString();
        }
    }
}
