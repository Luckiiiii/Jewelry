namespace Jewelry.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
    }
}
