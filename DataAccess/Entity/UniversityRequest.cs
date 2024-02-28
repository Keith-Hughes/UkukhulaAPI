using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class UniversityRequest(int RequestID, string university, string province, decimal amount, string status, DateTime dateCreated, string comment)
    {
        /*
        University.[Name] AS University, 
        Provinces.ProvinceName AS Province, 
        UniversityFundRequest.Amount,
        [dbo].[Status].[Type] AS [Status],
        UniversityFundRequest.DateCreated,
        UniversityFundRequest.Comment
         */
        public int RequestID { get; set; } = RequestID;
        public string University { get; set; } = university;
        public string Province { get; set; } = province;
        public decimal Amount { get; set; } = amount;
        public string Status { get; set; } = status;
        public DateTime DateCreated { get; set; } = dateCreated;
        public string Comment { get; set; } = comment;
    }
}
