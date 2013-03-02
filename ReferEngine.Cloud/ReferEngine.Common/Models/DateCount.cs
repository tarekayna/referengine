using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferEngine.Common.Models
{
    public class DateCount
    {
        public DateCount(DateTime dateTime, int count)
        {
            DateTime = dateTime;
            Count = count;
        }
        public DateTime DateTime { get; set; }
        public int Count { get; set; }
    }
}
