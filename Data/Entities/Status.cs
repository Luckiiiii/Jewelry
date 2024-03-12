namespace Jewelry.Data.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public StatusCategory StatusCategory { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CancellationDate { get; set; }
        public StoreUser User { get; set; }
    }
}
