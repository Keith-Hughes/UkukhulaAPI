using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AdminDAL(SqlConnection connection)
    {
        private readonly SqlConnection _connection = connection;

        /// <summary>
        /// Gets the all university fund requests.
        /// </summary>
        /// <returns>An IEnumerable&lt;UniversityRequest&gt;? .</returns>
        public IEnumerable<UniversityRequest>? GetAllUniversityFundRequests()
        {
            try
            {
                _connection.Open();
                List<UniversityRequest> requests = new List<UniversityRequest>();
                string query = "SELECT * FROM [dbo].[vw_UniversityRequests]";
                using (SqlCommand command = new SqlCommand(query, _connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UniversityRequest universityRequest = new UniversityRequest(
                                        reader.GetString(0),
                                        reader.GetString(1),
                                        reader.GetDecimal(2),
                                        reader.GetString(3),
                                        reader.GetDateTime(4),
                                        reader.GetString(5));
                        requests.Add(universityRequest);

                    }
                }
                _connection.Close();
                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting university requests connection problems{ex.Message}/n {ex.StackTrace}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public List<University> GetUniversities()
        {
            try
            {
                _connection.Open();
            List<University> universities = new List<University>();
            string query = "SELECT * FROM University";
            SqlDataReader reader = new SqlCommand(query, _connection).ExecuteReader();
            while (reader.Read())
            {
                University university = new(
                            _id: reader.GetInt32(0),
                           _name: reader.GetString(1),
                           _provinceID: reader.GetInt32(2),
                           _status:reader.GetString(3)
                           );

                universities.Add(university);
            }


            reader.Close();
            _connection.Close();

            return universities;
        }
            catch (Exception ex)
            {
                throw new Exception($"Error getting university requests connection problems{ex.Message}/n {ex.StackTrace}");
    }
            finally
            {
                _connection.Close();
            }
        }


        public UniversityRequest? UpdateUniversityFundRequest(int requestID, int statusID)
        {
            try
            {
                _connection.Open();
                UniversityRequest? request = null;
                string query = "EXEC [dbo].[usp_UpdateUniversityFundRequest] @RequestID, @StatusID";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@RequestID", requestID);
                    command.Parameters.AddWithValue("@StatusID", statusID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            request = new UniversityRequest(
                                                           reader.GetString(0),
                                                           reader.GetString(1),
                                                           reader.GetDecimal(2),
                                                           reader.GetString(3),
                                                           reader.GetDateTime(4),
                                                           reader.GetString(5)
                                                           );
                        }
                    }
                }
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating university fund request connection problems");
            }
            finally
            {
                _connection.Close();
            }
        }

        public UniversityRequest? NewUniversityFundRequest(int universityID, decimal amount, string comment)
        {
            try
            {
                _connection.Open();
                UniversityRequest? request = null;
                string query = "EXEC [dbo].[usp_NewUniversityFundRequest] @UniversityID, @Amount, @Comment";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@UniversityID", universityID);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@Comment", comment);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            request = new UniversityRequest(
                                        reader.GetString(0),
                                        reader.GetString(1),
                                        reader.GetDecimal(2),
                                        reader.GetString(3),
                                        reader.GetDateTime(4),
                                        reader.GetString(5)
                                        );
                        }
                    }
                }
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating new university fund request connection problems");
            }
            finally
            {
                _connection.Close();
            }
        }

        public IEnumerable<AllocationDetails>? GetUniversityFundAllocations()
        {
            try
            {
                _connection.Open();
                List<AllocationDetails> allocations = new List<AllocationDetails>();
                string query = "SELECT University.[Name] AS University, Provinces.ProvinceName AS Province, UniversityFundAllocation.Budget, UniversityFundAllocation.DateAllocated, ISNULL(SUM(StudentFundAllocation.Amount),0) AS TotalAllocated FROM University INNER JOIN UniversityFundAllocation ON University.ID = UniversityFundAllocation.UniversityID INNER JOIN Provinces ON University.ProvinceID = Provinces.ID LEFT JOIN StudentFundAllocation ON UniversityFundAllocation.ID = StudentFundAllocation.UniversityFundID WHERE DATEDIFF(YEAR, UniversityFundAllocation.DateAllocated, GETDATE()) = 0 GROUP BY University.[Name], Provinces.ProvinceName, UniversityFundAllocation.Budget, UniversityFundAllocation.DateAllocated";
                using (SqlCommand command = new SqlCommand(query, _connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AllocationDetails allocation = new AllocationDetails(
                                                   reader.GetString(0),
                                                   reader.GetString(1),
                                                   reader.GetDecimal(2),
                                                   reader.GetDateTime(3),
                                                   reader.GetDecimal(4)
                                                   );
                        allocations.Add(allocation);
                    }
                }
                return allocations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting university fund allocations connection problems");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Allocate()
        {
            new UniversityDAL(_connection).allocate();
        }
    }
}