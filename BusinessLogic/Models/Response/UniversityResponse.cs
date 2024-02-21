using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models.Response
{
    public class UniversityResponse
    {   
        public int ID { get; set; }
        public decimal AllocatedAmount {  get; set; }
        public DateOnly AllocationDate { get; set; }
        public decimal TotalAllocationUsed { get; set; }
        
    }
}
