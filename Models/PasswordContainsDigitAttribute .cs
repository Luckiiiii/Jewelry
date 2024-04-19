using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Jewelry.Models
{
    public class PasswordContainsDigitAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string password = value.ToString();
                if (!Regex.IsMatch(password, @"\d"))
                {
                    return new ValidationResult("Mật khẩu phải có ít nhất một chữ số ('0'-'9').");
                }
            }
            return ValidationResult.Success;
        }
    }

}
