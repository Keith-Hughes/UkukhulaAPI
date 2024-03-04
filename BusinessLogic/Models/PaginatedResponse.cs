using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    
        public class PaginatedResponse<T>
        {
            public IEnumerable<T> Data { get; set; }
            public int TotalPages { get; set; }
        }
    
}
