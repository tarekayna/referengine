﻿using System.Text;
using ReferEngine.Common.Email;
using ReferEngine.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ReferEngine.Common.Data
{
    internal static class DbConnector
    {
        private static IList<string> _connectionStrings;

        private static string GetConnectionString(string serverName, string databaseName)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = "tcp:" + serverName + ".database.windows.net,1433",
                    InitialCatalog = databaseName,
                    UserID = "tarek990@" + serverName,
                    Password = "r6g4d2hA..",
                    TrustServerCertificate = true,
                    MultipleActiveResultSets = true,
                    ConnectTimeout = 1800
                };
            return connectionStringBuilder.ConnectionString;
        }

        private static bool _initialized;

        private static void Initialize()
        {
            switch (Util.CurrentServiceConfiguration)
            {
                case Util.ReferEngineServiceConfiguration.ProductionCloud:
                    _connectionStrings = new List<string>
                    {
                        GetConnectionString("fnx5xvuqzn", "referengine_db"),
                        GetConnectionString("cy7xqbzm5w", "referengine_db_eastus"),
                        GetConnectionString("r0mmnh6n2q", "referengine_db_eastasia")
                    };
                    break;
                case Util.ReferEngineServiceConfiguration.TestCloud:
                    _connectionStrings = new List<string>
                    {
                        GetConnectionString("fnx5xvuqzn", "referengine_db_test"),
                        GetConnectionString("cy7xqbzm5w", "referengine_db_eastus_test"),
                        GetConnectionString("r0mmnh6n2q", "referengine_db_eastasia_test")
                    };
                    break;
                case Util.ReferEngineServiceConfiguration.Local:
                    _connectionStrings = new List<string>
                    {
                        GetConnectionString("fnx5xvuqzn", "referengine_db_local"),
                        GetConnectionString("cy7xqbzm5w", "referengine_db_eastus_local"),
                        GetConnectionString("r0mmnh6n2q", "referengine_db_eastasia_local")
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _initialized = true;
        }

        internal delegate Object ExecuteDelegate(DatabaseContext db);

        internal static Object Execute(ExecuteDelegate callback)
        {
            if (!_initialized)
            {
                Initialize();
            }

            for (int i = 0; i < _connectionStrings.Count; i++)
            {
                if (i == _connectionStrings.Count - 1)
                {
                    string connectionString = _connectionStrings[i];
                    using (var db = new DatabaseContext(connectionString))
                    {
                        return callback(db);
                    }
                }

                try
                {
                    string connectionString = _connectionStrings[i];
                    using (var db = new DatabaseContext(connectionString))
                    {
                        return callback(db);
                    }
                }
                catch (Exception e)
                {
                    StringBuilder builder = new StringBuilder();

                    Exception ex = e;

                    while (ex != null)
                    {
                        builder.AppendLine(string.Format("Message: {0}", e.Message));
                        builder.AppendLine(string.Format("Stack Trace: {0}", e.StackTrace));
                        builder.AppendLine();
                        ex = ex.InnerException;
                    }

                    ReferEmailer.SendPlainTextEmail("tarek@referengine.com", "Database Exception", builder.ToString());
                }
            }

            return new InvalidOperationException();
        }
    }
}
