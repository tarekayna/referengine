using System;
using System.Collections.Generic;
using System.Web.Helpers;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.StorageClient;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Tracing
{
    public enum TraceMessageCategory 
    {
        [StringValue("Error")]
        Error,

        [StringValue("Warning")]
        Warning,

        [StringValue("Success")]
        Success,

        [StringValue("Info")]
        Info,
    }

    public sealed class TraceMessage : TableEntity
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string RoleInstanceId { get; set; }
        public string RoleName { get; set; }
        public TraceMessageCategory Category 
        { 
            get
            {
                return string.IsNullOrEmpty(CategoryString) ? TraceMessageCategory.Info : (TraceMessageCategory)Enum.Parse(typeof(TraceMessageCategory), CategoryString);
            }
        }
        public string CategoryString { get; set; }
        private IDictionary<string, string> _properties;
        public string PropertiesString { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Properties
        {
            get
            {
                if (string.IsNullOrEmpty(PropertiesString))
                {
                    return new Dictionary<string, string>();
                }
                var result = (Dictionary<string, string>)Json.Decode(PropertiesString, typeof(Dictionary<string, string>));
                return result;
            }
        }

        public TraceMessage() { }
        public TraceMessage(string message, TraceMessageCategory category)
        {
            Message = message;
            CategoryString = category.GetStringValue();
            Time = DateTime.UtcNow;
            RoleInstanceId = RoleEnvironment.CurrentRoleInstance.Id;
            RoleName = RoleEnvironment.CurrentRoleInstance.Role.Name;
            PartitionKey = RoleEnvironment.CurrentRoleInstance.Role.Name;
            RowKey = (DateTime.MaxValue.Ticks - Time.Ticks).ToString("d19");
        }

        public TraceMessage AddProperty(string key, object value)
        {
            if (_properties == null) _properties = new Dictionary<string, string>();
            _properties.Add(key, value.ToString());
            PropertiesString = Json.Encode(_properties);
            return this;
        }

        public static TraceMessage Error(string message)
        {
            return new TraceMessage(message, TraceMessageCategory.Error);
        }

        public static TraceMessage Exception(Exception e)
        {
            TraceMessage traceMessage = new TraceMessage(e.Message, TraceMessageCategory.Error);
            traceMessage.AddProperty("Stack Trace", e.StackTrace);
            traceMessage.AddProperty("Source", e.Source);
            return traceMessage;
        }

        public static TraceMessage Warning(string message)
        {
            return new TraceMessage(message, TraceMessageCategory.Warning);
        }

        public static TraceMessage Success(string message)
        {
            return new TraceMessage(message, TraceMessageCategory.Success);
        }

        public static TraceMessage Info(string message)
        {
            return new TraceMessage(message, TraceMessageCategory.Info);
        }
    }
}