using System;
using System.Collections.Generic;
using AppSmarts.Common.Tracing;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.Data
{
    public static class DbConnector
    {
        private static IList<string> _connectionStringNames;
        private static bool _initialized;
        private static void EnsureInitialized()
        {
            if (_initialized) return;
            switch (Util.CurrentServiceConfiguration)
            {
                case Util.AppSmartsServiceConfiguration.ProductionCloud:
                    _connectionStringNames = new List<string> { "west", "east", "asia" };
                    break;
                case Util.AppSmartsServiceConfiguration.TestCloud:
                    _connectionStringNames = new List<string> { "west_test" };
                    break;
                case Util.AppSmartsServiceConfiguration.Local:
                    _connectionStringNames = new List<string> { "west_local" };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _initialized = true;
        }

        internal delegate Object ExecuteDelegate(DatabaseContext db);

        public static string GetCurretConnectionStringName()
        {
            EnsureInitialized();
            return _connectionStringNames[0];
        }

        internal static Object Execute(ExecuteDelegate callback)
        {
            EnsureInitialized();

            for (int i = 0; i < _connectionStringNames.Count; i++)
            {
                if (i == _connectionStringNames.Count - 1)
                {
                    using (var db = new DatabaseContext(_connectionStringNames[i]))
                    {
                        return callback(db);
                    }
                }

                try
                {
                    using (var db = new DatabaseContext(_connectionStringNames[i]))
                    {
                        return callback(db);
                    }
                }
                catch (Exception e)
                {
                    Tracer.Trace(TraceMessage.Exception(e));
                }
            }

            return new InvalidOperationException();
        }
    }
}
