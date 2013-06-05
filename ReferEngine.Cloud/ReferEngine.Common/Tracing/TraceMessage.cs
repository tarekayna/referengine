using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web.Helpers;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Table;
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
        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RowKey = (DateTime.MaxValue.Ticks - Time.Ticks).ToString("d19");
            }
        }
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
        }

        public TraceMessage AddProperty(string key, object value)
        {
            if (!string.IsNullOrEmpty(key) && value != null)
            {
                if (_properties == null) _properties = new Dictionary<string, string>();
                _properties.Add(key, value.ToString());
                PropertiesString = Json.Encode(_properties);
            }
            return this;
        }

        public static TraceMessage Error(string message)
        {
            return new TraceMessage(message, TraceMessageCategory.Error);
        }

        public static TraceMessage ExceptionWarning(Exception exception)
        {
            var traceMessage = Exception(exception);
            traceMessage.CategoryString = TraceMessageCategory.Warning.GetStringValue();
            return traceMessage;
        }

        public static TraceMessage Exception(Exception exception)
        {
            TraceMessage traceMessage = new TraceMessage(exception.Message, TraceMessageCategory.Error);
            int index = 1;
            var currentException = exception;

            while (currentException != null)
            {
                traceMessage.AddProperty("Message " + index, exception.Message);
                traceMessage.AddProperty("Stack Trace " + index, exception.StackTrace);
                traceMessage.AddProperty("Source " + index, exception.Source);
                index++;
                currentException = currentException.InnerException;
            }

            index = 1;
            if (exception is DbEntityValidationException)
            {
                var entityException = (DbEntityValidationException)exception;
                foreach (var error in entityException.EntityValidationErrors)
                {
                    foreach (var property in error.Entry.CurrentValues.PropertyNames)
                    {
                        var name = string.Format("EV-{0}-{1}", index, property);
                        traceMessage.AddProperty(name, error.Entry.CurrentValues[property]);
                    }

                    foreach (var dbValidationError in error.ValidationErrors)
                    {
                        var name = string.Format("EV-{0}-{1}", index, dbValidationError.PropertyName);
                        traceMessage.AddProperty(name, dbValidationError.ErrorMessage);
                    }

                    index++;
                }
            }

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