using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace AppSmarts.Common.Models
{
    public class RawTableData : TableEntity
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
