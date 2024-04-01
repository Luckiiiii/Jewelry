namespace Jewelry.Models
{
    public class CheckoutViewModel
    {
        public string CartData { get; set; }
        public string ConsigneeName { get; set; }
        public string DeliveryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
