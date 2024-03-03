
using System.ComponentModel.DataAnnotations;


namespace BusinessLogic.Models
{
    public class ExistingStudent
    {
        [Required]
        public int StudentID { get; set; }

        [Required]
        public byte Grade { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}