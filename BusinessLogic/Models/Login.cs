using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
