using System.ComponentModel.DataAnnotations;

namespace Jewelry.Models
{
    public class AddOrderViewModel
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int MaterialId { get; set; }
        public int PurityId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Tên người mua là trường bắt buộc.")]
        public string Customer { get; set; }
        [Required(ErrorMessage = "địa chỉ là trường bắt buộc.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Số điện thoại là trường bắt buộc.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Ghi chú là trường bắt buộc.")]
        public string Note { get; set; }
    }
}
