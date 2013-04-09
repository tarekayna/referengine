﻿using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using CloudStorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount;

namespace ReferEngine.Common.Tracing
{
    // Reference: http://www.windowsazure.com/en-us/develop/net/how-to-guides/table-services/#retrieve-all-entities

    public static class Tracer
    {
        private static string ConnectionString
        {
            get { return CloudConfigurationManager.GetSetting("TracerConnectionString"); }
        }

        private static string TableName
        {
            get { return "TraceData"; }
        }

        private static CloudTable _table;
        private static CloudTable Table
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
            TableOperation insertOperation = TableOperation.Insert(traceMessage);
            Table.Execute(insertOperation);
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

        public static IEnumerable<TraceMessage> GetTraceMessages(string role)
        {
            TableQuery<TraceMessage> query = new TableQuery<TraceMessage>();
            query.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, role));
            return Table.ExecuteQuery(query);
        }
    }
}
