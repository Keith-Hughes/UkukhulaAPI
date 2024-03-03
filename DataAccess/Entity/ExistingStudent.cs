
using System.ComponentModel.DataAnnotations;


namespace DataAccess.Entity
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
