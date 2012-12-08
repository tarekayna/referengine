using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ReferLib
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public Int64 FacebookId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Gender { get; set; }
        public int Timezone { get; set; }
        public bool Verified { get; set; }
        public string PictureUrl { get; set; }
        public bool PictureIsSilhouette { get; set; }

        public Person(dynamic person)
        {
            FacebookId = Convert.ToInt64(person.id);
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

        public static string Serialize(IList<Person> people)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(List<Person>));
            serializer.WriteObject(stream, people);
            var reader = new StreamReader(stream);
            stream.Position = 0;
            var serialized = reader.ReadToEnd();
            return serialized;
        }
    }
}
