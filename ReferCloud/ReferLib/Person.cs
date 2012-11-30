<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferLib
{
    public class Person
    {
        public int FacebookId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Timezone { get; set; }
        public bool Verified { get; set; }
        public string PictureUrl { get; set; }
        public bool PictureIsSilhouette { get; set; }

        public Person(dynamic person)
        {
            FacebookId = Convert.ToInt32(person.id);
            Name = person.name;
            FirstName = person.first_name;
            LastName = person.last_name;
            Email = person.email;
            Gender = person.gender;
            Timezone = Convert.ToInt32(person.timezone);
            Verified = Convert.ToBoolean(person.verified);
            PictureUrl = person.picture != null && person.picture.data != null ? person.picture.data.url : null;
            PictureIsSilhouette = person.picture != null && person.picture.data != null ? person.picture.data.is_silhouette : null;
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferLib
{
    public class Person
    {
        public int FacebookId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Timezone { get; set; }
        public bool Verified { get; set; }
        public string PictureUrl { get; set; }
        public bool PictureIsSilhouette { get; set; }

        public Person(dynamic person)
        {
            FacebookId = Convert.ToInt32(person.id);
            Name = person.name;
            FirstName = person.first_name;
            LastName = person.last_name;
            Email = person.email;
            Gender = person.gender;
            Timezone = Convert.ToInt32(person.timezone);
            Verified = Convert.ToBoolean(person.verified);
            PictureUrl = person.picture != null && person.picture.data != null ? person.picture.data.url : null;
            PictureIsSilhouette = person.picture != null && person.picture.data != null ? person.picture.data.is_silhouette : null;
        }
    }
}
>>>>>>> Facebook Post and Commit to DB
