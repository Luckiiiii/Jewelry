namespace Jewelry.Models
{
    public class SalesReportViewModel
    {
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public string Purity { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
