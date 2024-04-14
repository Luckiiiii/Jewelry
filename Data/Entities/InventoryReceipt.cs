namespace Jewelry.Data.Entities
{
    public class InventoryReceipt
    {
        public int Id { get; set; }
        public StoreUser User { get; set; }
        public Supplier Supplier { get; set; }
        public string? Note { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Confirmation { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public ICollection<InventoryReceiptDetails> Details { get; set; }
    }
}
