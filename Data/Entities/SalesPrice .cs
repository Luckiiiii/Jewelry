namespace Jewelry.Data.Entities
{
    public class SalesPrice
    {
        public int Id { get; set; }
        public ProductItem ProductItem { get; set; }
        public Decimal Price { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
