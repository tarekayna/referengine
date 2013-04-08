using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace ReferEngine.Common.Tracing
{
    public sealed class LogMessage : TableServiceEntity
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string RoleInstanceId { get; set; }
        public string RoleName { get; set; }
        public string Category { get; set; }
        public IDictionary<string, string> Properties { get; set; }

        public LogMessage() { }
        public LogMessage(string message, string category, IDictionary<string, string> properties = null)
        {
            Message = message;
            Category = category;
            Properties = properties ?? new Dictionary<string, string>();
            Time = DateTime.UtcNow;
            RoleInstanceId = RoleEnvironment.CurrentRoleInstance.Id;
            RoleName = RoleEnvironment.CurrentRoleInstance.Role.Name;
            PartitionKey = RoleEnvironment.DeploymentId;
            RowKey = (DateTime.MaxValue.Ticks - Time.Ticks).ToString("d19");
        }
    }
}