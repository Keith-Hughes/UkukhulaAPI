using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class UpdateStudentFundRequest
    {
        [Required]
        public byte Grade { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
