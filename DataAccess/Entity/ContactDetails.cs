using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataAccess.Entity
{
    public class ContactDetails
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(13)]
        public string PhoneNumber { get; set; }



    
    }
}
