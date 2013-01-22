using System.Collections.Generic;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ReferEngine.Common.Properties;

namespace ReferEngine.Common.Data
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

        public static App GetApp(long id)
        {
            App app = CacheOperations.GetApp(id);
            if (app == null)
            {
                using (var db = new ReferEngineDatabaseContext())
                {
                    app = db.Apps.First(a => a.Id == id);
                    app.Screenshots = db.AppScreenshots.Where(s => s.AppId == id).ToList();
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
                using (var db = new ReferEngineDatabaseContext())
                {
                    app = db.Apps.First(a => a.PackageFamilyName == packageFamilyName);
                    app.Screenshots = db.AppScreenshots.Where(s => s.AppId == app.Id).ToList();
                    CacheOperations.AddApp(packageFamilyName, app);
                }
            }
            return app;
        }

        public static void AddOrUpdateAppReceipt(AppReceipt appReceipt)
        {
            using (var db = new ReferEngineDatabaseContext())
            {
                AppReceipt existingReceipt = db.AppReceipts.Find(appReceipt.Id);
                if (existingReceipt == null)
                {
                    db.AppReceipts.Add(appReceipt);
                }
                else
                {
                    existingReceipt.PersonFacebookId = appReceipt.PersonFacebookId;
                }

                db.SaveChanges();
            }
        }

        public static AppReceipt GetAppReceipt(string id)
        {
            using (var db = new ReferEngineDatabaseContext())
            {
                return db.AppReceipts.Find(id);
            }
        }

        public static AppScreenshot GetAppScreenshot(long appId, string description)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                return db.AppScreenshots.First(s => s.AppId == appId && s.Description == description);
            }
        }

        public static Person GetPerson(Int64 facebookId)
        {
            Person person = CacheOperations.GetPerson(facebookId);
            if (person == null)
            {
                using (var db = new ReferEngineDatabaseContext())
                {
                    person = db.People.First(p => p.FacebookId == facebookId);
                    CacheOperations.AddPerson(person);
                }
            }

            return person;
        }

        public static AppRecommendation GetAppRecommdation(long appId, long personFacebookId)
        {
            using (var db = new ReferEngineDatabaseContext())
            {
                return db.AppRecommendations.FirstOrDefault(r => r.AppId == appId && r.PersonFacebookId == personFacebookId);
            }
        }

        public static void AddApp(App app)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                db.Apps.Add(app);
                db.SaveChanges();
            }
        }

        public static AppScreenshot AddAppScreenshot(AppScreenshot appScreenshot)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                AppScreenshot screenshot = db.AppScreenshots.Add(appScreenshot);
                db.SaveChanges();
                return screenshot;
            }
        }

        public static void AddAppReceipt(AppReceipt receipt)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                if (!db.AppReceipts.Any(r => r.Id == receipt.Id))
                {
                    db.AppReceipts.Add(receipt);
                    db.SaveChanges();
                }
            }
        }

        public static void AddOrUpdatePerson(Person person, ReferEngineDatabaseContext db)
        {
            Person existingPerson = db.People.FirstOrDefault(p => p.FacebookId == person.FacebookId);
            if (existingPerson == null)
            {
                db.People.Add(person);
            }
            else
            {
                existingPerson.Update(person);
            }
        }

        public static void AddOrUpdatePerson(Person person)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                AddOrUpdatePerson(person, db);
                db.SaveChanges();
            }
        }

        public static void AddCurrentUser(CurrentUser currentUser, BrokeredMessage message)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                AddOrUpdatePerson(currentUser.Person, db);

                foreach (var friend in currentUser.Friends)
                {
                    AddOrUpdatePerson(friend, db);

                    // Even if friendship exists, add it again so we track
                    db.Friendships.Add(new Friendship(currentUser.Person, friend));

                    TimeSpan renewLockDelta = Settings.Default.RenewLockDelta;
                    if (message.LockedUntilUtc.Subtract(renewLockDelta).CompareTo(DateTime.UtcNow) < 0)
                    {
                        message.RenewLock();
                    }
                }

                db.SaveChanges();
            }
        }

        public static void AddRecommendation(AppRecommendation recommendation)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                db.AppRecommendations.Add(recommendation);
                db.SaveChanges();
            }
        }

        public static void AddPrivateBetaSignup(PrivateBetaSignup privateBetaSignup)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                if (!db.PrivateBetaSignups.Any(s => s.Email == privateBetaSignup.Email))
                {
                    db.PrivateBetaSignups.Add(privateBetaSignup);
                    db.SaveChanges();   
                }
            }
        }

        public static void AddAppWebLink(AppWebLink appWebLink)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                if (!db.AppWebLinks.Any(l => l.Link == appWebLink.Link))
                {
                    db.AppWebLinks.Add(appWebLink);
                    db.SaveChanges();
                }
            }
        }

        public static void AddAppWebLinks(IList<AppWebLink> appWebLinks)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                foreach (var appWebLink in appWebLinks)
                {
                    if (!db.AppWebLinks.Any(l => l.Link == appWebLink.Link))
                    {
                        db.AppWebLinks.Add(appWebLink);
                    }
                }

                db.SaveChanges();
            }
        }
    }
}