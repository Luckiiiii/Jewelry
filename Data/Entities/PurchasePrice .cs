namespace Jewelry.Data.Entities
{
    public class PurchasePrice
    {
        public int Id { get; set; }
        public Decimal Price { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
