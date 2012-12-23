using ReferEngine.Common;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ReferEngine.Web.DataAccess
{
    public static class DatabaseOperations
    {
        private static SqlConnectionStringBuilder _connectionString;
        private static SqlConnection _connection;
        private static bool _initialized = false;

        private static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _connectionString =
                new SqlConnectionStringBuilder(
                    ConfigurationManager.ConnectionStrings["AzureReferEngine"].ConnectionString);
            _connection = new SqlConnection(_connectionString.ToString());

            _initialized = true;
        }

        public static void InsertPrivateBetaEmail(string email)
        {
            Initialize();
            var command = _connection.CreateCommand();
            command.CommandText = "InsertPrivateBetaEmail";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@email", email);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public static void AddApp(App app)
        {
            using (ReferDb db = new ReferDb())
            {
                db.Apps.Add(app);
                db.SaveChanges();
            }
        }

        public static App GetApp(long id)
        {
            App app = CacheOperations.GetApp(id);
            if (app == null)
            {
                using (var db = new ReferDb())
                {
                    app = db.Apps.First(a => a.Id == id);
                    CacheOperations.AddApp(id, app);
                }
            }
            return app;
        }

        public static App GetApp(string packageFamilyName)
        {
            App app = CacheOperations.GetApp(packageFamilyName);
            if (app == null)
            {
                using (var db = new ReferDb())
                {
                    app = db.Apps.First(a => a.PackageFamilyName == packageFamilyName);
                    CacheOperations.AddApp(packageFamilyName, app);
                }
            }
            return app;
        }

        //public static AppScreenshot AddAppScreenshot(AppScreenshot appScreenshot)
        //{
        //    using (ReferDb db = new ReferDb())
        //    {
        //        AppScreenshot screenshot = db.AppScreenshots.Add(appScreenshot);
        //        db.SaveChanges();
        //        return screenshot;
        //    }
        //}

        public static AppScreenshot GetAppScreenshot(long appId, string description)
        {
            using (ReferDb db = new ReferDb())
            {
                return db.AppScreenshots.First(s => s.AppId == appId && s.Description == description);
            }
        }

        //public static void AddAppReceipt(AppReceipt receipt)
        //{
        //    using (ReferDb db = new ReferDb())
        //    {
        //        if (!db.AppReceipts.Any(r => r.Id == receipt.Id))
        //        {
        //            db.AppReceipts.Add(receipt);
        //            db.SaveChanges();
        //        }
        //    }
        //}

        public static Person GetPerson(Int64 facebookId)
        {
            Person person = CacheOperations.GetPerson(facebookId);
            if (person == null)
            {
                using (var db = new ReferDb())
                {
                    person = db.People.First(p => p.FacebookId == facebookId);
                    CacheOperations.AddPerson(person);
                }
            }

            return person;
        }

        //public static void AddOrUpdatePerson(Person person, ReferDb db)
        //{
        //    Person existingPerson = db.People.FirstOrDefault(p => p.FacebookId == person.FacebookId);
        //    if (existingPerson == null)
        //    {
        //        db.People.Add(person);
        //    }
        //    else
        //    {
        //        existingPerson.Update(person);
        //    }
        //}

        //public static void AddOrUpdatePerson(Person person)
        //{
        //    using (ReferDb db = new ReferDb())
        //    {
        //        AddOrUpdatePerson(person, db);
        //        db.SaveChanges();
        //    }
        //}

        //public static Task AddPersonAndFriendsAsync(Person user, IList<Person> friends)
        //{
        //    return Task.Run(() => AddPersonAndFriends(user, friends));
        //}

        //public static void AddPersonAndFriends(Person user, IList<Person> friends)
        //{
        //    using (ReferDb db = new ReferDb())
        //    {
        //        AddOrUpdatePerson(user, db);

        //        foreach (var friend in friends)
        //        {
        //            AddOrUpdatePerson(friend, db);

        //            // Even if friendship exists, add it again so we track
        //            db.Friendships.Add(new Friendship(user, friend));
        //        }

        //        db.SaveChanges();
        //    }
        //}

        //public static void AddRecommendation(AppRecommendation recommendation)
        //{
        //    using (ReferDb db = new ReferDb())
        //    {
        //        db.AppRecommendations.Add(recommendation);
        //        db.SaveChanges();
        //    }
        //}
    }
}