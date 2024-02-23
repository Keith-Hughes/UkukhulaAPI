namespace BusinessLogic.Models.Response
{
    public class UserManagerResponse
    {
        public int? Id { get; set; } 
        public string Message {  get; set; }
        public bool isSuccess { get; set; }
        public string? Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Role { get; set; }
        public int? UniversityID {  get; set; }
    }
}
