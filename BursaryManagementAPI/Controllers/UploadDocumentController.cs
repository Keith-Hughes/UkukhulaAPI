using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using System;
using System.Threading.Tasks;

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
        public async Task<ActionResult> UploadDocument(int requestID, [FromForm] BusinessLogic.Models.UploadDocument uploadDocument)
        {
            try
            {
                return await _uploadDocumentBLL.UploadDocument(requestID, uploadDocument);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while uploading the document.");
            }
        }
    }
}
