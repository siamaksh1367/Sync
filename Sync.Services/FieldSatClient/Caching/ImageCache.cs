namespace Sync.Services.FieldSatClient.Caching
{
    public class ImageCache
    {
        public Guid FieldId { get; set; }
        public DateTime Date { get; set; }

        public ImageCache(DateTime date, Guid fieldId)
        {
            Date = date;
            FieldId = fieldId;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((ImageCache)obj);
        }

        public bool Equals(ImageCache other)
        {
            if (other == null)
                return false;

            return FieldId == other.FieldId && Date == other.Date;
        }

        public override int GetHashCode()
        {
            // Use a simple algorithm to combine the hash codes of the properties
            return HashCode.Combine(FieldId, Date);
        }

        public static bool operator ==(ImageCache left, ImageCache right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ImageCache left, ImageCache right)
        {
            return !(left == right);
        }
    }
}
