using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Register
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [AllowedValues("Student", "BBD Admin", "University Admin")]
        public string Role{ get; set; }


    }
}
