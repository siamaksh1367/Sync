using System.ComponentModel.DataAnnotations.Schema;

namespace Sync.DAL.Models
{
    public class Polygon
    {

        public Guid Id { get; set; }
        public int PointOrder { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Field Field { get; set; }
        [ForeignKey("Field")]
        public Guid FieldId { get; set; }
    }
}
