
using Microsoft.AspNetCore.Http;

using Microsoft.Data.SqlClient;

using Shared.Models;



namespace DataAccess
{
    public class UploadDocumentDAL(SqlConnection connection) : ConnectionHelper(connection)
    {
        

        public void updateDocument(int requestId, string documentType, IFormFile document)
        {
            SwitchConnection(true);
            string query = "";
            switch (documentType.ToLower())
            {
                case "cv":
                    query = "UPDATE Document SET CV = @Document WHERE RequestID = @RequestID";
                    break;

                case "transcript":
                    query = "UPDATE Document SET Transcript = @Document WHERE RequestID = @RequestID";
                    break;
                case "iddocument":
                    query = "UPDATE Document SET IDDocument = @Document WHERE RequestID = @RequestID";
                    break;
            }
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                using (var memoryStream = new MemoryStream())
                {
                    document.CopyTo(memoryStream);
                    command.Parameters.AddWithValue("@RequestID", requestId);
                    command.Parameters.AddWithValue("@Document", memoryStream.ToArray());
                    command.ExecuteNonQuery();
                }
            }
            SwitchConnection(false);

        }

        public UploadDocument getExistingDocuments(int requestId)
        {
            SwitchConnection(true) ;
            string query = "SELECT [CV], [IDDocument], [Transcript] FROM Document WHERE RequestID=@RequestId";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@RequestId", requestId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        byte[] cvData = (byte[])reader["CV"];
                        byte[] transcriptData = (byte[])reader["Transcript"];
                        byte[] idData = (byte[])reader["IDDocument"];
                        return new UploadDocument
                        {
                            
                            CV = ConvertToIFormFile(cvData, "CV.pdf"),
                            Transcript = ConvertToIFormFile(transcriptData, "Transcript.pdf"),
                            IDDocument = ConvertToIFormFile(idData, "ID.pdf")
                        };
                    }
                    return new UploadDocument { };
                }
            }

            }

        public bool documentsExits(UploadDocument document)
        {
            if(document.CV != null || document.Transcript != null || document.IDDocument != null)
            {
                return true;
            }
            return false;
        }


        private IFormFile ConvertToIFormFile(byte[] fileData, string fileName)
        {
            // Create an IFormFile from the byte array
            return new FormFile(new System.IO.MemoryStream(fileData), 0, fileData.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
        }
    }
}