using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferEngine.Common.Models.iOS
{
    public class iOSDataImport
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsFullImport { get; set; }
        public string DateString { get; set; }
    }
}
