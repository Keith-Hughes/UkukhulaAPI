namespace DataAccess.Entity
{
    public class UniversityFundRequest
    {
        int ID { get; set; }
        int UniversityID { get; set; }
        DateTime DateCreated { get; set; }

        decimal Amount { get; set; }
        int StatusID { get; set; }
        string Comment { get; set; }


        public UniversityFundRequest(int universityID, DateTime dateCreated, decimal amount, int statusID, string comment)
        {
            UniversityID = universityID;
            DateCreated = dateCreated;
            Amount = amount;
            StatusID = statusID;
            Comment = comment;
        }


       public int getUniversityID()=> UniversityID;
       
        public DateTime getDateCreated()=> DateCreated;
        public decimal getAmount()=> Amount;
        public int getStatusID()=> StatusID;
        public string getComment()=> Comment;

    }
}
