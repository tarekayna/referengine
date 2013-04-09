using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReferEngine.Common.Tracing;

namespace ReferEngine.Web.Models.Admin
{
    public class TraceLogsViewModel
    {
        public string Role { get; set; }
        public IEnumerable<TraceMessage> TraceMessages { get; set; }
        public int CurrentPage { get; set; }
    }
}