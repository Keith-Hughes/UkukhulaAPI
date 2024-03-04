
using DataAccess.Entity;
using Microsoft.Data.SqlClient;

namespace DataAccess
{
    public class AdminDAL(SqlConnection connection) : ConnectionHelper(connection)
    {
        
        private readonly SqlConnection _connection = connection;

        /// <summary>
        /// Gets the all university fund requests.
        /// </summary>
        /// <returns>An IEnumerable&lt;UniversityRequest&gt;? .</returns>
        /// 

        public BBDAllocation? getBudgetAndFunds(){
             return new UniversityDAL(connection).GetBBDAllocationByYear(DateTime.Now.Year);

        }

        public decimal getMoneySpentForYear(){
            decimal budget = 0;
            try
            {
                SwitchConnection(true);
                List<UniversityRequest> requests = new List<UniversityRequest>();
                string query = "SELECT * FROM [dbo].[vw_getMoneySpent]";
                
                using (SqlCommand command = new SqlCommand(query, _connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        budget += (decimal)reader.GetSqlMoney(0);

                    }
                }
                SwitchConnection(false);
                return budget;
            }
            catch (Exception ex)
            {
                SwitchConnection(false);
                return budget;
            }
           
            
            
        }
        /// 
        public void UpdateUniversityRequestStatus(int UniversityID, int StatusID)
        {
            /*IEnumerable<UniversityFundRequest> ufa =new UniversityDAL(connection).GetUniversityFundRequests();
            string query = "UPDATE [dbo].[UniversityFundRequest]";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UniversityUser? universityUser = new UniversityUser
                        {
                            UniversityName = reader.GetString(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Email = reader.GetString(4),
                            Status = reader.GetString(5),
                        };
                        universityUsers.Add(universityUser);
                    }
                }
                catch (Exception e)
                {
                    SwitchConnection(false);
                }
            }
            SwitchConnection(false);*/

        }
        public IEnumerable<UniversityRequest>? GetAllUniversityFundRequests(int offset, int pageSize)
        {
            try
            {
                SwitchConnection(true);
                List<UniversityRequest> requests = new List<UniversityRequest>();
                string query = "SELECT * FROM [dbo].[vw_UniversityRequests] ORDER BY RequestID OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UniversityRequest universityRequest = new UniversityRequest(
                                            reader.GetInt32(0),
                                            reader.GetString(1),
                                            reader.GetString(2),
                                            reader.GetDecimal(3),
                                            reader.GetString(4),
                                            reader.GetDateTime(5),
                                            reader.GetString(6));
                            requests.Add(universityRequest);
                        }
                    }
                }
                SwitchConnection(false);
                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting university requests connection problems{ex.Message}/n {ex.StackTrace}");
            }
            finally
            {
                SwitchConnection(false);
            }
        }


        public int updateUserActivity(int UserID,string Status)
        {
            Dictionary<string, string> responseUpdateUser = null;
            SwitchConnection(true);
            
            
            string query = "UPDATE [dbo].[User] SET Status= @Status WHERE ID = @UserID ";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@Status", Status.ToUpper());
                    command.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataReader reader = command.ExecuteReader();
                    
                }
                catch (Exception e)
                {
                    
                    SwitchConnection(false);
                    return 0;
                }
            }
            SwitchConnection(false);

            return 1;

        }

        public List<UniversityUser> GetUniversityUsers()
        {
           List<UniversityUser> universityUsers = [];
            SwitchConnection(true);
            
            
            string query = "SELECT * FROM [dbo].[GetAllUsers]";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UniversityUser? universityUser = new UniversityUser
                        {
                            ID =  reader.GetInt32(0),
                            UniversityName = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            PhoneNumber = reader.GetString(4),
                            Email = reader.GetString(5),
                            Status = reader.GetString(6),
                        };
                        universityUsers.Add(universityUser);
                    }
                }
                catch (Exception e)
                {
                    SwitchConnection(false);
                }
            }
            SwitchConnection(false);

            return universityUsers;

        }

        public List<University> GetUniversities()
        {
            try
            {
                SwitchConnection(true);
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
            SwitchConnection(false);

            return universities;
        }
            catch (Exception ex)
            {
                SwitchConnection(false);
                throw new Exception($"Error getting university requests connection problems{ex.Message}/n {ex.StackTrace}");
    }
            finally
            {
                SwitchConnection(false);
            }
        }


        public Dictionary<string,string> UpdateUniversityFundRequest(int requestID, int statusID)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            try
            {
                
                SwitchConnection(true);
                UniversityRequest? request = null;
                string query = "UPDATE UniversityFundRequest SET StatusID = @StatusID WHERE ID = @RequestID;";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@RequestID", requestID);
                    command.Parameters.AddWithValue("@StatusID", statusID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        response.Add("message", "Upated successfully");
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Add("message", "Error Updating");
            }
            finally
            {
                SwitchConnection(false);
                
            }
            return response;
        }


        public Dictionary<string, string> RejectUniversityFundRequest(int requestID, int statusID, string comment)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            try
            {

                SwitchConnection(true);
                string query = "UPDATE UniversityFundRequest SET StatusID = @StatusID, Comment = @Comment WHERE ID = @RequestID;";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@RequestID", requestID);
                    command.Parameters.AddWithValue("@StatusID", statusID);
                    command.Parameters.AddWithValue("@Comment", comment);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        response.Add("message", "Rejection Sent Successfully");
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Add("message", "Error Updating");
            }
            finally
            {
                SwitchConnection(false);

            }
            return response;
        }



        public UniversityRequest? NewUniversityFundRequest(int universityID, decimal amount, string comment)
        {
            try
            {
                SwitchConnection(true);
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
                                        reader.GetInt32(0),
                                        reader.GetString(1),
                                        reader.GetString(2),
                                        reader.GetDecimal(3),
                                        reader.GetString(4),
                                        reader.GetDateTime(5),
                                        reader.GetString(6)
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
                SwitchConnection(false);
            }
        }

        public int GetTotalUniversityRequestsCount()
        {
            try
            {
                SwitchConnection(true);
                string query = "SELECT COUNT(*) FROM [dbo].[vw_UniversityRequests]";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    int totalCount = (int)command.ExecuteScalar();
                    return totalCount;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting total count of university requests: {ex.Message}");
            }
            finally
            {
                SwitchConnection(false);
            }
        }

        public IEnumerable<AllocationDetails>? GetUniversityFundAllocations()
        {
            try
            {
                SwitchConnection(true);
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
                SwitchConnection(false);
            }
        }
        public List<UniversityFundAllocation> AllocateProjection(){
             List<UniversityFundAllocation> universityFundAllocations= new UniversityDAL(_connection).AllocateProjection();
           
            return universityFundAllocations;
        }

        public int Allocate()
        {   
            
           return new UniversityDAL(_connection).allocate();
            
        }
    }
}