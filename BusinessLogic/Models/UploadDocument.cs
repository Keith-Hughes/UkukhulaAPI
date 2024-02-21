using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Models
{
    public class UploadDocument
    {
        
        public required IFormFile File { get; set; }
        public int DocumentType { get; set; }

    }
}
