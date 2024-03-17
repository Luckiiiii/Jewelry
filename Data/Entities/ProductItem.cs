namespace Jewelry.Data.Entities
{
    public class ProductItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Size Sizes { get; set; }
        public Material Materials { get; set; }
        public ICollection<PurchasePrice> PurchasePrice { get; set; }
        public ICollection<SalesPrice> SalesPrice { get; set; }
        public int Quantity { get; set; }
    }
}
