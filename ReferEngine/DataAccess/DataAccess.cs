using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ReferEngine.DataAccess
{
    public static class DataAccess
    {
        private static SqlConnectionStringBuilder _connectionString;
        private static SqlConnection _connection;
        private static bool _initialized = false;
        private static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _connectionString =
                new SqlConnectionStringBuilder(
                    ConfigurationManager.ConnectionStrings["AzureReferEngine"].ConnectionString);
            _connection = new SqlConnection(_connectionString.ToString());

            _initialized = true;
        }

        public static void InsertPrivateBetaEmail(string email)
        {
            Initialize();
            var command = _connection.CreateCommand();
            command.CommandText = "InsertPrivateBetaEmail";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@email", email);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}