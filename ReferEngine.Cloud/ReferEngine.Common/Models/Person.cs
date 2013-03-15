using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class Person
    {
        public Person() { }

        public Person(bool isJohnSmith = false)
        {
            if (isJohnSmith)
            {
                Name = "A Customer";
                FirstName = "John";
                LastName = "Smith";
                FacebookId = 0;
                PictureUrl = "https://fbcdn-profile-a.akamaihd.net/static-ak/rsrc.php/v2/yo/r/UlIqmHJn-SK.gif";
            }
        }

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
            NumberOfFriends = 0;
        }

        [DataMember]
        public Int64 FacebookId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public int Timezone { get; set; }

        [DataMember]
        public bool Verified { get; set; }

        [DataMember]
        public string PictureUrl { get; set; }

        [DataMember]
        public int NumberOfFriends { get; set; }

        [DataMember]
        public bool PictureIsSilhouette { get; set; }

        public void Update(Person latestPerson)
        {
            if (this.FacebookId != latestPerson.FacebookId)
            {
                throw new InvalidOperationException();
            }
            
            Email = latestPerson.Email ?? this.Email;
            FirstName = latestPerson.FirstName ?? FirstName;
            Gender = latestPerson.Gender ?? Gender;
            LastName = latestPerson.LastName ?? LastName;
            Name = latestPerson.Name ?? Name;
            PictureIsSilhouette = latestPerson.PictureUrl != null ? latestPerson.PictureIsSilhouette : PictureIsSilhouette;
            PictureUrl = latestPerson.PictureUrl ?? PictureUrl;
            Verified = latestPerson.Verified;
            NumberOfFriends = latestPerson.NumberOfFriends;
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
