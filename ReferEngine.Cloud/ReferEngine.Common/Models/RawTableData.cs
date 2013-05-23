using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace ReferEngine.Common.Models
{
    public class RawTableData : TableEntity
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
