using Microsoft.Extensions.Options;
using Moq;
using Sync.Common.Options;
using Sync.Services.FieldSatClient;

namespace Tests_Sync_Services
{
    public class FieldSatHttpClientTests
    {

        [Fact]
        public async void GetFieldsAsync_CollectData_ShouldReturnResponse()
        {
            //Arrange
            var options = new Mock<IOptions<HttpEndPoints>>();
            options.Setup(o => o.Value)
                .Returns(new HttpEndPoints { FeilSatFieldEndPoint = new Uri("https://field-sat.cordulus.dev/api/v1/fields") });
            var httpClient = new Mock<IHttpClientFactory>();
            var client = new HttpClient();
            httpClient.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var fieldSatHttpClient = new FieldSatHttpClient(httpClient.Object, options.Object);

            //Act
            var response = await fieldSatHttpClient.GetFieldsAsync();

            //Assert
            Assert.True(response.Count() > 0);
        }

        [Fact]
        public async void GetImagessAsync_CollectData_ShouldReturnResponse()
        {
            //Arrange
            var options = new Mock<IOptions<HttpEndPoints>>();

            options.Setup(o => o.Value)
                .Returns(new HttpEndPoints { FeilSatImagesEndPoint = new Uri("https://field-sat.cordulus.dev/api/v1/fields/{fieldId}/images") });
            var httpClient = new Mock<IHttpClientFactory>();
            var client = new HttpClient();
            httpClient.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var fieldSatHttpClient = new ImageSatHttpClient(httpClient.Object, options.Object);

            //Act
            var response = await fieldSatHttpClient.GetImagesAsync(new Guid("0ba2e0a7-01c6-4dbb-9a62-ecde75f105d6"),
                                                                    new Dictionary<string, string>() {
                                                                        { "since", "2022-01-01" },
                                                                        { "until", "2023-01-01" } });
            //Assert
            Assert.True(response.Count() > 0);
        }
    }
}