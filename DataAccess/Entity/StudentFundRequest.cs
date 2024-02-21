namespace DataAccess.Entity
{
    public class StudentFundRequest
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UniversityName { get; set; }
        public string IDNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Age { get; set; }
        public string GenderName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RaceName { get; set; }
        public byte Grade { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestCreatedDate { get; set; }
        public string FundRequestStatus { get; set; }
        public string DocumentStatus { get; set; }
        public string Comment { get; set; }
    }

}
