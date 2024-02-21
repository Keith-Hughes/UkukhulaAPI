using Azure.Core;
using Azure.Storage.Blobs;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UploadDocumentDAL
    {
        private readonly SqlConnection _connection;
        private readonly BlobServiceClient _blobServiceClient;

        public UploadDocumentDAL(SqlConnection connection, BlobServiceClient blobServiceClient)
        {
            _connection = connection;
            _blobServiceClient = blobServiceClient;
        }
        public async Task<ActionResult> UploadDocument(int requestID, UploadDocument uploadDocument)
        {
            try
            {
                var file = uploadDocument.File;
                if (file == null || file.Length == 0)
                    return new BadRequestObjectResult("File is empty");

                await _connection.OpenAsync();

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                var blobClient = _blobServiceClient.GetBlobContainerClient("bursarymanagementcontainer").GetBlobClient(uniqueFileName);
                await blobClient.UploadAsync(file.OpenReadStream());

                string query = "INSERT INTO Document (RequestID, TypeID, Document) VALUES (@RequestID, @DocumentType, @DocumentPath)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@RequestID", requestID);
                    command.Parameters.AddWithValue("@DocumentType", uploadDocument.DocumentType);
                    command.Parameters.AddWithValue("@DocumentPath", blobClient.Uri.ToString());
                    await command.ExecuteNonQueryAsync();
                }

                return new OkObjectResult("File uploaded successfully!");
            }
            finally
            {
                if (_connection.State != System.Data.ConnectionState.Closed)
                    _connection.Close();
            }
        }

    }
}
