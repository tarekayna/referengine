using ReferLib;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ReferEngine.DataAccess
{
    public static class DataOperations
    {
        private static SqlConnectionStringBuilder _connectionString;
        private static SqlConnection _connection;
        private static readonly ReferDb ReferDb = new ReferDb();
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

        public static bool TryGetApp(int id, out App app)
        {
            try
            {
                app = ReferDb.Apps.First(a => a.Id == id);
                return true;
            }
            catch (InvalidOperationException)
            {
                app = null;
                return false;
            }
        }

        public static void AddPerson(Person person)
        {
            if (ReferDb.People.Count(p => p.FacebookId == person.FacebookId) == 0)
            {
                ReferDb.People.Add(person);
                ReferDb.SaveChanges();   
            }
        }
    }
}