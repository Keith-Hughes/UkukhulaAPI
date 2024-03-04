using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TokenDAL(SqlConnection connection) : ConnectionHelper(connection)
    {
        public void saveToDatabase(string token, DateTime expirationDate)
        {
            string insertQuery = "INSERT INTO Tokens (Token, ExpirationDate) VALUES (@Token, @ExpirationDate)";
            SwitchConnection(true);
            using (SqlCommand command = new SqlCommand(insertQuery, _connection))
            {
                command.Parameters.AddWithValue("@Token", token);
                command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                command.ExecuteNonQuery();
            }
            SwitchConnection(false);
        }

        public Dictionary<string, DateTime> getTokens(string tokenToCheck)
        {   
            string selectQuery = "SELECT * FROM Tokens WHERE Token = @Token";
            Dictionary<string, DateTime> Tokens = new Dictionary<string, DateTime>();
            SwitchConnection(true);
            using (SqlCommand command = new SqlCommand(selectQuery, _connection))
            {
                command.Parameters.AddWithValue("@Token", tokenToCheck);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Token found in the database, check expiration date if needed
                        DateTime storedExpirationDate = (DateTime)reader["ExpirationDate"];
                        string token = (string)reader["Token"];
                        Tokens[token] = storedExpirationDate;
                        SwitchConnection(false);
                        return Tokens;
                        // Perform additional checks or return the result as needed
                    }
                    else
                    {   
                        SwitchConnection(false);
                        return null;
                    }
                }
            }
        }
    }
}
