using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email {  get; set; }

    }
}
