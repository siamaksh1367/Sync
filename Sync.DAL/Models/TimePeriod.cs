namespace Sync.DAL.Models
{
    public class TimePeriod
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Field> Fields { get; set; }

    }
}
