using Jewelry.Data.Entities;

namespace Jewelry.Models
{
    public class InventoryReportViewModel
    {
        public int SelectedProductId { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }
        public List<InventoryReceiptDetails> Report { get; set; }
    }
}
