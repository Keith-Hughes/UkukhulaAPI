using Microsoft.AspNetCore.Http;

namespace Shared.Models
{
    public class UploadDocument
    {
        public IFormFile ?CV { get; set; }
        public IFormFile ?Transcript { get; set; } 
        public IFormFile ?IDDocument { get; set; } 
    }
}