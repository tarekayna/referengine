using System.Collections.Generic;
using AppSmarts.Common.Tracing;

namespace AppSmarts.Web.Models.Admin
{
    public class TraceLogsViewModel
    {
        public string Role { get; set; }
        public IEnumerable<TraceMessage> TraceMessages { get; set; }
        public int CurrentPage { get; set; }
    }
}