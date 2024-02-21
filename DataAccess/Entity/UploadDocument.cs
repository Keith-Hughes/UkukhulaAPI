using Microsoft.AspNetCore.Http;

namespace DataAccess.Entity
{
    public class UploadDocument
    {
        
        public required IFormFile File { get; set; }
        public int DocumentType { get; set; }

    }
}
