using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;


namespace BusinessLogic
{
    public class UploadDocumentBLL
    {
        private readonly UploadDocumentDAL _uploadDocumentDAL;

        public UploadDocumentBLL(UploadDocumentDAL uploadDocumentDAL)
        {
            _uploadDocumentDAL = uploadDocumentDAL;
        }

        public async Task<UploadDocument> UploadDocument(int requestID, UploadDocument uploadDocument)
        {
            
                if (uploadDocument.CV != null)
                {
                    _uploadDocumentDAL.updateDocument(requestID, "CV", uploadDocument.CV);
                }
                if (uploadDocument.Transcript != null)
                {
                    _uploadDocumentDAL.updateDocument(requestID, "Transcript", uploadDocument.Transcript);

                }
                if (uploadDocument.IDDocument != null)
                {
                    _uploadDocumentDAL.updateDocument(requestID, "IDDocument", uploadDocument.IDDocument);

                }
            
            
                return _uploadDocumentDAL.getExistingDocuments(requestID);
        }

        public IFormFile GetFile(int requestID, string DocumentType)
        {
            UploadDocument currentDocs = _uploadDocumentDAL.getExistingDocuments(requestID);
            switch (DocumentType.ToLower())
            {
                case "cv":
                    return currentDocs.CV;
                case "transcript":
                    return currentDocs.Transcript;
                case "iddocument":
                    return currentDocs.IDDocument;

            }
            return null;
        }


    }
}
