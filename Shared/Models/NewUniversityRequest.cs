using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Models
{
    public class NewUniversityRequest
    {
        [Required]
        public string firstName {  get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string contactNumber { get; set; }
        [Required]
        public string universityName { get; set; }
        [Required]
        public string province { get; set; }
        [Required]
        public decimal amount { get; set; }
    }
}
