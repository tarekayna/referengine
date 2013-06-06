using System;
using System.Collections.Generic;
using AppSmarts.Common.Tracing;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.Data.iOS
{
    public static class iOSDatabaseConnector
    {
        private static IList<string> _connectionStringNames;
        private static bool _initialized;
        private static void EnsureInitialized()
        {
            if (_initialized) return;
            switch (Util.CurrentServiceConfiguration)
            {
                case Util.AppSmartsServiceConfiguration.ProductionCloud:
                case Util.AppSmartsServiceConfiguration.Local:
                    _connectionStringNames = new List<string> { "ios_west" };
                    break;
                case Util.AppSmartsServiceConfiguration.TestCloud:
                    _connectionStringNames = new List<string> { "ios_west_test" };
                    break;
                //case Util.ReferEngineServiceConfiguration.Local:
                //    _connectionStringNames = new List<string> { "ios_local" };
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _initialized = true;
        }

        internal delegate Object ExecuteDelegate(iOSDatabaseContext db);

        internal static Object Execute(ExecuteDelegate callback)
        {
            EnsureInitialized();

            for (int i = 0; i < _connectionStringNames.Count; i++)
            {
                if (i == _connectionStringNames.Count - 1)
                {
                    using (var db = new iOSDatabaseContext(_connectionStringNames[i]))
                    {
                        return callback(db);
                    }
                }

                try
                {
                    using (var db = new iOSDatabaseContext(_connectionStringNames[i]))
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
