using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DataAccess.Entity;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic
{
    public class UploadDocumentBLL
    {
        private readonly UploadDocumentDAL _uploadDocumentDAL;

        public UploadDocumentBLL(UploadDocumentDAL uploadDocumentDAL)
        {
            _uploadDocumentDAL = uploadDocumentDAL;
        }

        public async Task<ActionResult> UploadDocument(int requestID, Models.UploadDocument uploadDocument)
        {
            try
            {
                UploadDocument upload = new()
                {
                    File = uploadDocument.File,
                    DocumentType = uploadDocument.DocumentType
                };
                return await _uploadDocumentDAL.UploadDocument(requestID, upload);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading document: {ex.Message}");
            }
        }
    }
}
