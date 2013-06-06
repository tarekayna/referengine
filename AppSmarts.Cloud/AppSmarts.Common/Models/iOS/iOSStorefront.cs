using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSStorefront
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        public const int NumberOfFields = 4;

        public static IEnumerable<iOSStorefront> CreateMany(string[] fields)
        {
            List<iOSStorefront> result = new List<iOSStorefront>();
            for (int i = 0; i < fields.Length; i += NumberOfFields)
            {
                var current = new iOSStorefront
                {
                    ExportDate = Util.EpochPlusMilliseconds(fields[i]),
                    Id = Convert.ToInt32(fields[i + 1]),
                    CountryCode = fields[i + 2],
                    Name = fields[i + 3]
                };
                result.Add(current);
            }
            return result;
        }
    }
}
