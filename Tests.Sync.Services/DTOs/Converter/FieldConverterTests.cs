using Sync.Services.DTOs;
using Sync.Services.DTOs.Converter;
using System.Text.Json;

namespace Tests.Sync.Services.DTOs.Converter
{
    public class FieldConverterTests
    {
        [Fact]
        public void Read_ParseAPolygon_ShouldReturnAListOfPoint()
        {
            // Arrange
            var json = "\"POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))\"";
            var converter = new FieldConverter();
            var options = new JsonSerializerOptions();
            var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            reader.Read();

            // Act
            var points = converter.Read(ref reader, typeof(IEnumerable<PointDto>), options);

            // Assert
            Assert.NotNull(points);
            var pointsList = points.ToList();
            Assert.Equal(5, pointsList.Count);
            Assert.Equal(30, pointsList[0].Longitude);
            Assert.Equal(10, pointsList[0].Latitude);
            Assert.Equal(40, pointsList[1].Longitude);
            Assert.Equal(40, pointsList[1].Latitude);
            Assert.Equal(20, pointsList[2].Longitude);
            Assert.Equal(40, pointsList[2].Latitude);
            Assert.Equal(10, pointsList[3].Longitude);
            Assert.Equal(20, pointsList[3].Latitude);
            Assert.Equal(30, pointsList[4].Longitude);
            Assert.Equal(10, pointsList[4].Latitude);
        }
    }
}
