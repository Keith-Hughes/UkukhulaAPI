using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class UpdateStudentFundRequest
    {
        [Required]
        public byte Grade { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
