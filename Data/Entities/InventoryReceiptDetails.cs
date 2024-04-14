using Microsoft.AspNetCore.Components;

namespace Jewelry.Data.Entities
{
    public class InventoryReceiptDetails
    {
        public int Id { get; set; }
        public ProductItem ProductItem { get; set; }
        public InventoryReceipt InventoryReceipt { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
