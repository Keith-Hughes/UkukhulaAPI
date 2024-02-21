
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class UniversityFundRequestExisting
    {

        int ID { get; set; }
        string UniversityName { get; set; }
        decimal Amount { get; set; }
        string Status { get; set; }
        string Province { get; set; }
        string Comment { get; set; }
        DateTime DateCreated { get; set; }

        public UniversityFundRequestExisting(int id, string universityName, decimal amount, string status, string province, string comment, DateTime dateCreated)
        {
            ID = id;
            UniversityName = universityName;
            Amount = amount;
            Status = status;
            Province = province;
            Comment = comment;
            DateCreated = dateCreated;
        }

        public int getID() => ID;
        public string getUniversityName() => UniversityName;
        public decimal getAmount() => Amount;
        public string getStatus() => Status;
        public string getProvince() => Province;
        public string getComment() => Comment;
        public DateTime getDateCreated() => DateCreated;

    }
}

