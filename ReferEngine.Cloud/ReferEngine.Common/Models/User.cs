﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        public List<App> Apps { get; set; }
    }
}
