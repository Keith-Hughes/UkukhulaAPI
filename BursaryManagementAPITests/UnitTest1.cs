using Azure.Storage.Blobs;
using BursaryManagementAPI.Controllers;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Moq;

namespace BursaryManagementAPITests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var mockFile = new Mock<IFormFile>();
            UploadDocument uploadDocument = new UploadDocument { File = mockFile.Object,
            DocumentType = 1};


            var mockDAL = new Mock<UploadDocumentDAL>();
            var mockBLL = new Mock<UploadDocumentBLL>(mockDAL.Object);

            
            var mockBlobService = new Mock<BlobServiceClient>();
            var controller = new UploadDocumentController(mockBLL.Object);
            
            mockDAL.Setup(m => m).Returns(mockDAL.Object);


            
            var result = controller.UploadDocument(1, new UploadDocument
            {
                File = mockFile.Object,
                DocumentType = 1

            });
            Assert.True(result.IsCompletedSuccessfully);
        }
    }
}