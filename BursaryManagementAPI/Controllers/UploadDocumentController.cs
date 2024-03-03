using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using Shared.Models;

namespace BursaryManagementAPI.Controllers
{
    /// <summary>
    /// The upload document controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UploadDocumentController : ControllerBase
    {
        private readonly UploadDocumentBLL _uploadDocumentBLL;

        public UploadDocumentController(UploadDocumentBLL uploadDocumentBLL)
        {
            _uploadDocumentBLL = uploadDocumentBLL;
        }

        [HttpPost("{requestID}/upload")]
        public async Task<ActionResult> UploadDocument(int requestID, [FromForm] UploadDocument uploadDocument)
        {
            try
            {
                // Check if any file is provided
                if (uploadDocument.CV == null && uploadDocument.Transcript == null && uploadDocument.IDDocument == null)
                {
                    return BadRequest("No files provided for upload.");
                }
                return Ok(_uploadDocumentBLL.UploadDocument(requestID, uploadDocument));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{requestID}/{DocumentType}")]
        public IActionResult getDocument(int requestID, string DocumentType) 
        {
            try
            {
                IFormFile file = _uploadDocumentBLL.GetFile(requestID, DocumentType);
                Console.WriteLine(file.Length);
                if(file == null || file.Length <= 4)
                {
                    return NotFound("No Documents found");
                }
                return File(file.OpenReadStream(), file.ContentType, file.FileName );
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}