namespace Jewelry.Models
{
    public class CartViewModel
    {
        public int productItemId { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
        public int sizeId { get; set; }
        public string sizeName { get; set; }
        public int purityId { get; set; }
        public string purityName { get; set; }
        public int materialId { get; set; }
        public string materialName { get; set; }
        public decimal weight { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}
