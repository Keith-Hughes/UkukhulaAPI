
using DataAccess.Entity;
using Microsoft.Data.SqlClient;
using System.Data;



public class UniversityDAL(SqlConnection connection)
{
    SqlConnection _connection = connection;

    /// <summary>
    /// Switches the connection.
    /// </summary>
    /// <param name="mustBeOpen">If true, must be open.</param>
    private void SwitchConnection(bool mustBeOpen)
    {
        switch (_connection.State)
        {
            case ConnectionState.Open:
                if (mustBeOpen) break;
                _connection.Close();
                break;
            case ConnectionState.Closed:
                if (mustBeOpen) _connection.Open();
                break;
        }
    }

    //public GetAmountLeftForUniversity(int UniversityID)
    //{

    //}

    public List<University> GetUniversities()
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
                       _provinceID: reader.GetInt32(2)
                       );

            universities.Add(university);
        }

        
        reader.Close();
        SwitchConnection(false);

        return universities;
    }

    public void allocate()
    {
        SwitchConnection(true);
        List<int> universityIDs = GetUniversities().Select(u => u.GetID()).Where(u => u.s).ToList();
        int numberOfInstitutions = universityIDs.Count();
        BBDAllocation? allocation = GetBBDAllocationByYear(DateTime.Now.Year);
        if (allocation == null)
        {
            throw new Exception("No allocation for the year");
        }

        decimal budget = allocation.getBudget() / numberOfInstitutions;
        universityIDs.ForEach(id =>
        {
            var universityFundAllocation = new UniversityFundAllocation(budget, DateTime.Now, id, allocation.getID());
            SaveUniversityFundAllocation(universityFundAllocation);
        });
        SwitchConnection(false);

    }


    public List<BBDAllocation> BBDAllocations()
    {

        SwitchConnection(true);
        string query = "SELECT * FROM BBDAllocation";
        SqlDataReader reader = new SqlCommand(query, _connection).ExecuteReader();
        List<BBDAllocation> allocations = new List<BBDAllocation>();

        while (reader.Read())
        {

            BBDAllocation allocation = new(
                        _id: reader.GetInt32(0),
                        _budget: (decimal)reader.GetSqlMoney(1),
                        _dateCreated: reader.GetDateTime(2)
                        );
            Console.WriteLine(allocation.getBudget());
            allocations.Add(allocation);
        }
        reader.Close();
        SwitchConnection(false);
        return allocations;
    }

    // GET BBD ALLOCATION BY YEAR
    public BBDAllocation? GetBBDAllocationByYear(int Year) => BBDAllocations()[0];


    public void SaveUniversityFundAllocation(UniversityFundAllocation allocation)
    {
        SwitchConnection(true);
        string query = "INSERT INTO UniversityFundAllocation (Budget, DateAllocated, UniversityID, BBDAllocationID) VALUES (@Budget, @DateAllocated, @UniversityID, @BBDAllocationID)";
        using (SqlCommand command = new SqlCommand(query, _connection))
        {
            Console.WriteLine(allocation.getBudget());
            command.Parameters.AddWithValue("@Budget", allocation.getBudget());
            command.Parameters.AddWithValue("@DateAllocated", allocation.getDateAllocated());
            command.Parameters.AddWithValue("@UniversityID", allocation.getUniversityID());
            command.Parameters.AddWithValue("@BBDAllocationID", allocation.getBBDAllocationID());
            command.ExecuteNonQuery();
        }
        SwitchConnection(false);
    }

    public void SaveUniversityFundRequest(UniversityFundRequest request)
    {   SwitchConnection(true);
        string query = "INSERT INTO UniversityFundRequest (UniversityID, DateCreated, Amount, StatusID, Comment) VALUES (@UniversityID, @DateCreated, @Amount, @StatusID, @Comment)";
        using (SqlCommand command = new SqlCommand(query, _connection))
        {
            command.Parameters.AddWithValue("@UniversityID", request.getUniversityID());
            command.Parameters.AddWithValue("@DateCreated", request.getDateCreated());
            command.Parameters.AddWithValue("@Amount", request.getAmount());
            command.Parameters.AddWithValue("@StatusID", request.getStatusID());
            command.Parameters.AddWithValue("@Comment", request.getComment());
            command.ExecuteNonQuery();
        }
        SwitchConnection (false);
    }

    public List<UniversityFundRequest> GetUniversityFundRequests()
    {
        SwitchConnection(true);
        List<UniversityFundRequest> requests = new List<UniversityFundRequest>();
        string query = "SELECT * FROM UniversityFundRequest";
        SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader();

        while (reader.Read())
        {
            UniversityFundRequest request = new(
                                      universityID: reader.GetInt32(0),
                                      dateCreated: reader.GetDateTime(2),
                                      amount: (decimal)reader.GetSqlMoney(3),
                                      statusID: reader.GetString(4),
                                      comment: reader.GetString(5)
                                                                                                                                                                                 );
            requests.Add(request);
        }

        reader.Close();
        SwitchConnection(false);

        return requests;
    }

    public void UpdateUniversityFundRequestStatusAndComment(int id, string statusID, string comment)
    {
        SwitchConnection (true);
        string query = "UPDATE UniversityFundRequest SET StatusID = @StatusID, Comment = @Comment WHERE ID = @ID";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@StatusID", statusID);
        command.Parameters.AddWithValue("@Comment", comment);
        command.Parameters.AddWithValue("@ID", id);
        command.ExecuteNonQuery();
        SwitchConnection (false);
    }
}

