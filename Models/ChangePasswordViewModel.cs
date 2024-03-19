using System.ComponentModel.DataAnnotations;

namespace Jewelry.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là trường bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu mới phải dài ít nhất là {2} và tối đa {1} ký tự..", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp.")]
        [Required(ErrorMessage = "Xác nhận mật khẩu mới là trường bắt buộc.")]
        public string ConfirmPassword { get; set; }
    }
}
