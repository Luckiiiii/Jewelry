using System.ComponentModel.DataAnnotations;

namespace Jewelry.Models
{
    public class ContactViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "too short")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [MaxLength(250)]
        public string Message { get; set; }

    }
}



