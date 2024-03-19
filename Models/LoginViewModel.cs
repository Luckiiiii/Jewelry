using System.ComponentModel.DataAnnotations;

namespace Jewelry.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email là trường bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
