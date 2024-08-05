namespace Sync.DAL.Models
{
    public class Field
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Polygon> Polygons { get; set; }
        public ICollection<TimePeriod> TimePeriods { get; set; }
    }
}
