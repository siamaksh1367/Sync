using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sync.Services.DTOs.Converter
{
    public class FieldConverter : JsonConverter<IEnumerable<PointDto>>
    {
        public override IEnumerable<PointDto>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var polygonString = reader.GetString();

            if (string.IsNullOrWhiteSpace(polygonString))
            {
                return Enumerable.Empty<PointDto>();
            }

            var points = polygonString
                .Replace("POLYGON ((", "")
                .Replace("))", "")
                .Split(',')
                .Select(part =>
                {
                    var coords = part.Trim().Split(' ');
                    if (coords.Length == 2 && double.TryParse(coords[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude) &&
                        double.TryParse(coords[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude))
                    {
                        return new PointDto
                        {
                            Latitude = latitude,
                            Longitude = longitude
                        };
                    }
                    return null;
                })
                .Where(p => p != null)
                .ToList();

            return points;
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<PointDto> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
