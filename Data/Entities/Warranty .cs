namespace Jewelry.Data.Entities
{
    public class Warranty
    {
        public int Id { get; set; }
        public StoreUser User { get; set; }
        public string WarrantyInformation { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Note { get; set; }
        public Order Order { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Decimal Price { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? EmployeeName { get; set; }
    }
}
