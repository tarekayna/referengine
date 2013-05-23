using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.WindowsAzure;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using CloudStorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount;

namespace ReferEngine.Common.Tracing
{
    // Reference: http://www.windowsazure.com/en-us/develop/net/how-to-guides/table-services/#retrieve-all-entities

    public static class Tracer
    {
        private static string _connectionString;
        private static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = CloudConfigurationManager.GetSetting("TracerConnectionString");
                }
                return _connectionString;
            }
        }

        private static string TableName
        {
            get { return "TraceData"; }
        }

        private static SpinLock _tableLock = new SpinLock();

        private static CloudTable _table;
        public static CloudTable Table
        {
            get
            {
                if (_table == null)
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                    CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                    _table = tableClient.GetTableReference(TableName);
                    _table.CreateIfNotExists();
                }
                return _table;
            }
        }

        public static void Trace(TraceMessage traceMessage)
        {
            bool gotLock = false;
            try
            {
                _tableLock.Enter(ref gotLock);
                traceMessage.Time = DateTime.UtcNow;
                Table.Execute(TableOperation.Insert(traceMessage));
                Console.WriteLine(traceMessage.Message);
            }
            finally 
            {
                if (gotLock) _tableLock.Exit();
            }
        }

        private static IList<string> _roles;
        public static IList<string> GetRoles(bool forceRefresh = false)
        {
            if (forceRefresh || _roles == null)
            {
                TableQuery<TraceMessage> query = new TableQuery<TraceMessage>();
                query.Select(new List<string> {"RoleName"});
                _roles = new List<string>();
                foreach (TraceMessage traceMessage in Table.ExecuteQuery(query))
                {
                    if (!_roles.Contains(traceMessage.RoleName))
                    {
                        _roles.Add(traceMessage.RoleName);
                    }
                }
            }
            return _roles;
        }

        public static IEnumerable<TraceMessage> GetTraceMessages(string role, int page, int requestedPageSize)
        {
            const int maxPageSize = 500;
            int pageSize = requestedPageSize > maxPageSize ? maxPageSize : requestedPageSize;
            int take = pageSize * page;
            int skip = pageSize * (page - 1);
            var query = new TableQuery<TraceMessage>()
                                .Take(take)
                                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, role));
            IEnumerable<TraceMessage> list = new List<TraceMessage>(Table.ExecuteQuery(query));
            if (list.Count() <= skip)
            {
                return new List<TraceMessage>();
            }
            list = list.Skip(skip);
            return list;
        }
    }
}
