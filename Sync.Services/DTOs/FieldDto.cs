namespace Sync.Services.DTOs
{
    public class FieldDto
    {
        public Guid Id { get; set; }
        public IEnumerable<PointDto> Polygon { get; set; }
    }
}