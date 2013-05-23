using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferEngine.Common.Data
{
    internal class ServiceBusAccessInfo
    {
        public string Issuer { get; set; }
        public string Namespace { get; set; }
        public string Key { get; set; }
    }
}
