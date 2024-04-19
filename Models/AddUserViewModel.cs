using System.ComponentModel.DataAnnotations;

namespace Jewelry.Models
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Email là trường bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Họ là trường bắt buộc.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Tên là trường bắt buộc.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Địa chỉ là trường bắt buộc.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "số điện thoại là trường bắt buộc.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [DataType(DataType.Password)]
        [PasswordContainsDigit]
        public string Password { get; set; }
        [Required(ErrorMessage = "Xác nhận mật khẩu là trường bắt buộc.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        public bool IsEmployee { get; set; } 
    }
}
