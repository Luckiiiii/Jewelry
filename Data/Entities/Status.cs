namespace Jewelry.Data.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public int StatusCategoryId { get; set; }
        public StatusCategory StatusCategory { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UserId { get; set; }
        public StoreUser User { get; set; }
        public string? Note { get; set; }
    }
}
