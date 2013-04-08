using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Diagnostics;
using System.Data.Services.Client;

namespace ReferEngine.Common.Tracing
{
    public class TableTraceListener : TraceListener
    {
        private string GetConfigurationAttribute(string name)
        {
            return Attributes[name];
        }

        private string ConnectionString
        {
            get { return GetConfigurationAttribute("ConnectionString"); }
        }

        private string _tableName;
        private string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(_tableName))
                {
                    _tableName = GetConfigurationAttribute("TableName");
                }
                return _tableName;
            }
        }

        private TableServiceContext _context;
        private TableServiceContext Context
        {
            get
            {
                if (_context == null)
                {
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);
                    CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                    cloudTableClient.CreateTableIfNotExist(TableName);
                    _context = cloudTableClient.GetDataServiceContext();
                    _context.MergeOption = MergeOption.NoTracking;
                }
                return _context;
            }
        }

        protected override string[] GetSupportedAttributes() { return new[] { "ConnectionString", "TableName" }; }

        public override void Write(string message, string category)
        {
            Context.AddObject(TableName, new LogMessage(message, category));
            Context.SaveChangesWithRetries();
        }

        public override void WriteLine(string message, string category) { Write(message + "\n", category); }
        public override void Write(string message) { Write(message, null); }
        public override void WriteLine(string message) { Write(message + "\n"); }
    }
}
