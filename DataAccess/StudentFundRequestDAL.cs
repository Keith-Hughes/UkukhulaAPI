using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess
{
    public class StudentFundRequestDAL(SqlConnection connection) : ConnectionHelper(connection)
    {
        private readonly SqlConnection _connection = connection;

        public IEnumerable<StudentFundRequest> GetAllRequests()
        {
            try
            {
                SwitchConnection(true);
                List<StudentFundRequest> requests = new List<StudentFundRequest>();
                string query = "EXEC [dbo].[GetStudentFundRequests]";
                using (SqlCommand command = new SqlCommand(query, _connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudentFundRequest request = new StudentFundRequest
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            UniversityName = reader.GetString(reader.GetOrdinal("UniversityName")),
                            IDNumber = reader.GetString(reader.GetOrdinal("IDNumber")),
                            BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                            Age = reader.GetByte(reader.GetOrdinal("Age")),
                            GenderName = reader.GetString(reader.GetOrdinal("GenderName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            RaceName = reader.GetString(reader.GetOrdinal("RaceName")),
                            Grade = reader.GetByte(reader.GetOrdinal("Grade")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            RequestCreatedDate = reader.GetDateTime(reader.GetOrdinal("RequestCreatedDate")),
                            FundRequestStatus = reader.GetString(reader.GetOrdinal("FundRequestStatus")),
                            DocumentStatus = reader.GetString(reader.GetOrdinal("DocumentStatus")),
                            Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? "" : reader.GetString(reader.GetOrdinal("Comment"))
                        };
                        requests.Add(request);
                    }
                }
                SwitchConnection(false);
                
                return requests;
            }
            finally
            {
                SwitchConnection(false);
            }
        }

        public IEnumerable<StudentFundRequest> GetStudentFundRequestsByUniversity(int universityId)
        {
            try
            {
                SwitchConnection(true);
                List<StudentFundRequest> requests = new List<StudentFundRequest>();
                string query = "EXEC [dbo].[GetStudentFundRequestsByUniversity] @UniversityID";
                using (SqlCommand command = new SqlCommand(query, _connection)) {
                    command.Parameters.AddWithValue("@UniversityID", universityId);

       
                using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentFundRequest request = new StudentFundRequest
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                UniversityName = reader.GetString(reader.GetOrdinal("UniversityName")),
                                IDNumber = reader.GetString(reader.GetOrdinal("IDNumber")),
                                BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                                Age = reader.GetByte(reader.GetOrdinal("Age")),
                                GenderName = reader.GetString(reader.GetOrdinal("GenderName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                RaceName = reader.GetString(reader.GetOrdinal("RaceName")),
                                Grade = reader.GetByte(reader.GetOrdinal("Grade")),
                                Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                RequestCreatedDate = reader.GetDateTime(reader.GetOrdinal("RequestCreatedDate")),
                                FundRequestStatus = reader.GetString(reader.GetOrdinal("FundRequestStatus")),
                                DocumentStatus = reader.GetString(reader.GetOrdinal("DocumentStatus")),
                                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? "" : reader.GetString(reader.GetOrdinal("Comment"))
                            };
                            requests.Add(request);
                        }
                    }
                    SwitchConnection(false);

                    return requests;
                }
            }
            finally
            {
                SwitchConnection(false);
            }
        }

        public int getUniversityFundAllocationByID(int id)
        {
            try
            {
                SwitchConnection(true);
                int UniversityAllocationID = 0;
                string query = "SELECT ID FROM UniversityFundAllocation WHERE UniversityID=@UniversityID AND YEAR(DateAllocated) = YEAR(GETDATE())";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@UniversityID", id);
                    UniversityAllocationID = (int)command.ExecuteScalar();
                    return UniversityAllocationID;
                }
            }catch(Exception ex)
            {
                SwitchConnection(false);
                return 0;
            }
        }

        public void Create(CreateStudentFundRequestForNewStudent newRequest)
        {
            try
            {
                SwitchConnection(true);

                string query = "EXEC [dbo].[CreateStudentFundRequestForNewStudent] @IDNumber,@GenderName,@RaceName,@UniversityID,@BirthDate,@Grade,@Amount,@UserID, @UniversityFundID";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@IDNumber", newRequest.IDNumber);
                command.Parameters.AddWithValue("@GenderName", newRequest.GenderName);
                command.Parameters.AddWithValue("@RaceName", newRequest.RaceName);
                command.Parameters.AddWithValue("@UniversityID", newRequest.UniversityID);
                command.Parameters.AddWithValue("@BirthDate", newRequest.BirthDate);
                command.Parameters.AddWithValue("@Grade", newRequest.Grade);
                command.Parameters.AddWithValue("@Amount", newRequest.Amount);
                command.Parameters.AddWithValue("@UserID", newRequest.UserID);
                command.Parameters.AddWithValue("@UniversityFundID", getUniversityFundAllocationByID(newRequest.UniversityID));
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                
                Console.WriteLine($"This is the catch: {e.Message}\n This is stackTrace: {e.StackTrace}");
                SwitchConnection(false);
            }
            finally
            {
                SwitchConnection(false);
            }
        }

        public void CreateForExistingStudent(ExistingStudent newRequest)
        {
            try
            {
                SwitchConnection(true);
                string query = "INSERT INTO [dbo].[StudentFundRequest] ([Grade], [Amount], [Comment], [StudentID], [StatusID])VALUES (@Grade, @Amount, '', @StudentID, 3)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@StudentID", newRequest.StudentID);
                    command.Parameters.AddWithValue("@Grade", newRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", newRequest.Amount);

                    command.ExecuteNonQuery();
                    SwitchConnection(false);
                }
            }
            finally
            {
                SwitchConnection(false);
            }
        }

        public void UpdateRequest(int id, UpdateStudentFundRequest updatedRequest)
        {
            try
            {
                SwitchConnection(true);
                string query = "UPDATE StudentFundRequest SET Grade = @Grade, Amount = @Amount WHERE ID = @ID AND StatusID = 3";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Grade", updatedRequest.Grade);
                    command.Parameters.AddWithValue("@Amount", updatedRequest.Amount);
                    command.Parameters.AddWithValue("@ID", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        SwitchConnection(false);
                        throw new KeyNotFoundException("Student fund request not found!");
                    }
                }
            }
            finally
            {
                SwitchConnection(false);
            }
        }

        public StudentFundRequest UpdateApplicationStatus(int applicationId, int status, string comment)
        {
            try
            {
                SwitchConnection(true);
                if (status == 2 && string.IsNullOrWhiteSpace(comment))
                {
                    return new StudentFundRequest
                    {
                        ID = 0,
                        Comment = "A comment is required when changing the status to Rejected."
                    };
                }

                string query;

                if (!string.IsNullOrWhiteSpace(comment))
                {
                    query = "UPDATE StudentFundRequest SET StatusID = @Status, Comment = @Comment WHERE ID = @ID";
                }
                else
                {
                    query = "UPDATE StudentFundRequest SET StatusID = @Status WHERE ID = @ID";
                }

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ID", applicationId);

                    if (!string.IsNullOrWhiteSpace(comment))
                    {
                        command.Parameters.AddWithValue("@Comment", comment);
                    }

                    command.ExecuteNonQuery();
                     StudentFundRequest studentFundRequestDetails = GetAllRequests().ToList().First(request => request.ID == applicationId);

                    SwitchConnection(false);
                    return studentFundRequestDetails;
                }
            }
            catch (SqlException ex)
            {
                SwitchConnection(false);
                return new StudentFundRequest
                {
                    ID = 0,
                    Comment = ex.Message
                };
            }
            finally
            {
                SwitchConnection(false);
            }
        }
    }
}