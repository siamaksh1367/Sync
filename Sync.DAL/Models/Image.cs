using System.ComponentModel.DataAnnotations.Schema;

namespace Sync.DAL.Models
{

    public class Image
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Uri Link { get; set; }
        public Field Field { get; set; }
        [ForeignKey("Field")]
        public Guid FieldId { get; set; }

    }
}
