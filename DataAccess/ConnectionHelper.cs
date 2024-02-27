using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public abstract class ConnectionHelper
    {
        private readonly SqlConnection _connection;
        protected ConnectionHelper(SqlConnection _connection) { 
        this._connection = _connection;
        }

        public void SwitchConnection(bool mustBeOpen)
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
    }
}
