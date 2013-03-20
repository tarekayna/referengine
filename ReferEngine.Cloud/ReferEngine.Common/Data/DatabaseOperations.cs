using System.Data.Entity;
using System.Diagnostics;
using Itenso.TimePeriod;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using ReferEngine.Common.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Membership = ReferEngine.Common.Models.Membership;

namespace ReferEngine.Common.Data
{
    public static class DatabaseOperations
    {
        public static App GetApp(long id)
        {
            App app = CacheOperations.GetApp(id);
            if (app == null)
            {
                using (var db = new ReferEngineDatabaseContext())
                {
                    app = db.Apps.Where(a => a.Id == id && a.IsActive)
                             .Include(a => a.RewardPlan)
                             .First();
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
                    app = db.Apps.Where(a => a.PackageFamilyName == packageFamilyName && a.IsActive)
                             .Include(a => a.RewardPlan)
                             .First();
                    app.Screenshots = db.AppScreenshots.Where(s => s.AppId == app.Id).ToList();
                    CacheOperations.AddApp(packageFamilyName, app);
                }
            }
            return app;
        }

        public static App GetApp(string packageFamilyName, string appVerificationCode)
        {
            App app = CacheOperations.GetApp(packageFamilyName, appVerificationCode);
            if (app == null)
            {
                using (var db = new ReferEngineDatabaseContext())
                {
                    app = db.Apps.Where(a => a.PackageFamilyName == packageFamilyName && 
                                             a.VerificationCode == appVerificationCode &&
                                             a.IsActive)
                                 .Include(a => a.RewardPlan)
                                 .First();
                    app.Screenshots = db.AppScreenshots.Where(s => s.AppId == app.Id).ToList();
                    CacheOperations.AddApp(packageFamilyName, appVerificationCode, app);
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
                return db.AppReceipts.SingleOrDefault(r => r.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
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
                    person = db.People.FirstOrDefault(p => p.FacebookId == facebookId);

                    if (person == null)
                    {
                        Trace.TraceWarning("GetPerson returned null: " + facebookId);
                    }
                    else
                    {
                        CacheOperations.AddPerson(person);
                    }
                }
            }

            return person;
        }

        public static AppRecommendation GetAppRecommdation(long appId, long personFacebookId)
        {
            using (var db = new ReferEngineDatabaseContext())
            {
                return
                    db.AppRecommendations.FirstOrDefault(r => r.AppId == appId && r.PersonFacebookId == personFacebookId);
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

        public static void AddPersonAndFriends(Person person, IList<Person> friends, BrokeredMessage message)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                person.NumberOfFriends = friends.Count();
                AddOrUpdatePerson(person, db);

                foreach (var friend in friends)
                {
                    AddOrUpdatePerson(friend, db);

                    // Even if friendship exists, add it again so we track
                    db.Friendships.Add(new Friendship(person, friend));

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
                var existing =
                    db.AppRecommendations.FirstOrDefault(r => r.FacebookPostId == recommendation.FacebookPostId);
                if (existing != null) return;
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

        public static void AddOrUpdateAppWebLinks(IList<AppWebLink> appWebLinks)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                const int take = 1000;
                int skip = 0;

                while (true)
                {
                    var currentSet = appWebLinks.Skip(skip).Take(take);
                    skip += take;
                    foreach (var appWebLink in currentSet)
                    {
                        AppWebLink existing = db.AppWebLinks.SingleOrDefault(l => l.Link == appWebLink.Link);
                        if (existing == null)
                        {
                            db.AppWebLinks.Add(appWebLink);
                        }
                        else
                        {
                            existing.LastUpdated = DateTime.UtcNow;
                        }
                    }

                    db.SaveChanges();

                    if (currentSet.Count() < take)
                    {
                        break;
                    }
                }
            }
        }

        public static IList<AppWebLink> GetAppWebLinks()
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                return db.AppWebLinks.ToList();
            }
        }

        public static void AddStoreAppInfo(StoreAppInfo storeAppInfo)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                StoreAppInfo existing = db.StoreAppInfos.FirstOrDefault(i => i.MsAppId.Equals(storeAppInfo.MsAppId));
                if (existing == null)
                {
                    db.StoreAppInfos.Add(storeAppInfo);
                    db.SaveChanges();
                }
                else if (!existing.IsIdentical(storeAppInfo))
                {
                    db.StoreAppInfos.Remove(existing);
                    db.StoreAppInfos.Add(storeAppInfo);
                    db.SaveChanges();
                }
            }
        }

        public static ConfirmationCodeModel GetConfirmationCodeModel(string email)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var tokens = from m in db.Memberships
                             join u in db.Users on m.UserId equals u.Id
                             select new {u.Email, u.FirstName, m.ConfirmationToken};
                var token = tokens.FirstOrDefault(i => i.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (token == null) return null;
                var confirmationCodeModel = new ConfirmationCodeModel
                                                {
                                                    Email = token.Email,
                                                    ConfirmationCode = token.ConfirmationToken,
                                                    FirstName = token.FirstName
                                                };
                return confirmationCodeModel;
            }
        }

        public static User GetUser(string email)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                User user = db.Users.First(c => c.Email == email);
                user.Apps = db.Apps.Where(a => a.UserId == user.Id && a.IsActive).ToList();
                return user;
            }
        }

        public static User GetUser(int id)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                User user = db.Users.First(c => c.Id == id);
                user.Apps = db.Apps.Where(a => a.UserId == user.Id && a.IsActive).ToList();
                return user;
            }
        }

        public static User GetUserFromConfirmationCode(string code)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                Membership membership =
                    db.Memberships.First(m => m.ConfirmationToken.Equals(code, StringComparison.OrdinalIgnoreCase));
                User user = db.Users.First(u => u.Id == membership.UserId);
                user.Apps = db.Apps.Where(a => a.UserId == user.Id && a.IsActive).ToList();
                return user;
            }
        }

        public static Membership GetMembership(string email)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    var membership = db.Memberships.First(m => m.UserId == user.Id);
                    if (membership != null)
                    {
                        return membership;
                    }
                }

                return null;
            }
        }

        public static string GetRole(int userId)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var userInRole = db.UsersInRoles.First(u => u.UserId == userId);
                var role = db.Roles.First(r => r.RoleId == userInRole.RoleId);
                return role.RoleName;
            }
        }

        public static IList<UserMembership> GetUserMemberships()
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                IList<UserMembership> result = new List<UserMembership>();
                foreach (var user in db.Users)
                {
                    var membership = db.Memberships.First(m => m.UserId == user.Id);
                    string role = GetRole(user.Id);
                    result.Add(new UserMembership(user, membership, role));
                }
                return result;
            }
        }

        public static AppRecommendation GetAppRecommendation(long facebookPostId)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                return db.AppRecommendations.FirstOrDefault(r => r.FacebookPostId == facebookPostId);
            }
        }

        public static IList<AppRecommendation> GetAppRecommendations(long appId, int count = -1)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var result = from r in db.AppRecommendations
                             where r.AppId == appId
                             orderby r.DateTime descending
                             select r;

                return count > -1 ? result.Take(count).ToList() : result.ToList();
            }
        }

        public static void AddUserRole(User user, string roleName)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                Role role = db.Roles.First(r => r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));
                db.UsersInRoles.Add(new UserInRole {RoleId = role.RoleId, UserId = user.Id});
                db.SaveChanges();
            }
        }

        public static List<PersonRecommendationUnitResult> GetAppRecommendationsPeople(App app, TimeRange timeRange, string who)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var result = new List<PersonRecommendationUnitResult>();
                switch (who)
                {
                    case "recommended":
                        var recommendations = from r in db.AppRecommendations
                                              where app.Id == r.AppId &&
                                                    timeRange.Start.CompareTo(r.DateTime) < 0 &&
                                                    timeRange.End.CompareTo(r.DateTime) > 0
                                              orderby r.DateTime descending
                                              select r;
                        foreach (AppRecommendation appRecommendation in recommendations)
                        {
                            var person = GetPerson(appRecommendation.PersonFacebookId) ?? new Person(true);
                            var location = appRecommendation.IpAddress == null ? null : GetIpAddressLocation(appRecommendation.IpAddress);
                            var res = new PersonRecommendationUnitResult(person, location, appRecommendation);
                            result.Add(res);
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                return result;
            }
        }

        public static int GetNumberOfFriends(Person person)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                return db.Friendships.Count(f => f.Person1FacebookId == person.FacebookId);
            }
        }


        public static List<ChartUnitResult> GetAppActionCount(App app, TimeRange timeRange, TimeSpan timeSpan, string who)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var result = new List<ChartUnitResult>();

                TimeRange currentRange = new TimeRange(timeRange.Start, timeRange.Start.Add((timeSpan)));
                while (!currentRange.HasInside(timeRange.End))
                {
                    DateTime rangeStart = currentRange.Start;
                    DateTime rangeEnd = currentRange.End;

                    int count = 0;
                    switch (who)
                    {
                        case "launched":
                            var auths = from a in db.AppAuthorizations
                                        where app.Id == a.App.Id &&
                                              rangeStart.CompareTo(a.TimeStamp) < 0 &&
                                              rangeEnd.CompareTo(a.TimeStamp) > 0
                                        select a;

                            count = auths.Count();
                            break;
                        case "intro":
                            var intros = from v in db.RecommendationPageViews
                                         where v.AppId == app.Id &&
                                               rangeStart.CompareTo(v.TimeStamp) < 0 &&
                                               rangeEnd.CompareTo(v.TimeStamp) > 0
                                         select v;

                            count = intros.Count();
                            break;
                        case "recommended":
                            var recs = from r in db.AppRecommendations
                                      where app.Id == r.AppId &&
                                            rangeStart.CompareTo(r.DateTime) < 0 &&
                                            rangeEnd.CompareTo(r.DateTime) > 0
                                      select r;
                            count = recs.Count();
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    string str = string.Empty;
                    if (timeSpan.Equals(TimeSpan.FromDays(1)))
                    {
                        str = currentRange.Start.Date.ToShortDateString();
                    }
                    else if (timeSpan.Equals(TimeSpan.FromHours(1)))
                    {
                        str = currentRange.Start.ToShortTimeString();
                    }

                    ChartUnitResult unitResult = new ChartUnitResult
                    {
                        Desc = str,
                        Result = count
                    };
                    result.Add(unitResult);

                    currentRange.Move(timeSpan);
                }

                return result;
            }
        }

        public static List<MapUnitResult> GetAppActionLocations(App app, TimeRange timeRange, string who)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var result = new List<MapUnitResult>();
                switch (who)
                {
                    case "launched":
                        {
                            var locations = from a in db.AppAuthorizations
                                            join l in db.IpAddressLocations on a.UserHostAddress equals l.IpAddress
                                            where app.Id == a.App.Id &&
                                                  timeRange.Start.CompareTo(a.TimeStamp) < 0 &&
                                                  timeRange.End.CompareTo(a.TimeStamp) > 0
                                            group l by new {l.Country, l.Region, l.City}
                                            into crc
                                            select new {crc.Key.Country, crc.Key.Region, crc.Key.City, Count = crc.Count()};

                            result.AddRange(locations.Select(l => new MapUnitResult
                            {
                                City = l.City,
                                Region = l.Region,
                                Country = l.Country,
                                Result = l.Count
                            }));
                            break;
                        }
                    case "intro":
                        {
                            var locations = from v in db.RecommendationPageViews
                                            join l in db.IpAddressLocations on v.IpAddress equals l.IpAddress
                                            where app.Id == v.AppId &&
                                                  timeRange.Start.CompareTo(v.TimeStamp) < 0 &&
                                                  timeRange.End.CompareTo(v.TimeStamp) > 0 &&
                                                  v.RecommendationPage == RecommendationPage.Intro
                                            group l by new {l.Country, l.Region, l.City}
                                            into crc
                                            select new {crc.Key.Country, crc.Key.Region, crc.Key.City, Count = crc.Count()};

                            result.AddRange(locations.Select(l => new MapUnitResult
                            {
                                City = l.City,
                                Region = l.Region,
                                Country = l.Country,
                                Result = l.Count
                            }));
                            break;
                        }
                    case "recommended":
                        {
                            var locations = from r in db.AppRecommendations
                                            join l in db.IpAddressLocations on r.IpAddress equals l.IpAddress
                                            where app.Id == r.AppId &&
                                                  timeRange.Start.CompareTo(r.DateTime) < 0 &&
                                                  timeRange.End.CompareTo(r.DateTime) > 0
                                            group l by new {l.Country, l.Region, l.City}
                                            into crc
                                            select new {crc.Key.Country, crc.Key.Region, crc.Key.City, Count = crc.Count()};

                            result.AddRange(locations.Select(l => new MapUnitResult
                            {
                                City = l.City,
                                Region = l.Region,
                                Country = l.Country,
                                Result = l.Count
                            }));
                            break;
                        }
                    default:
                        throw new InvalidOperationException();
                }
                return result;
            }
        }

        public static IList<StoreAppInfo> FindStoreApps(string term, int count)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                var lowercaseTerm = term.ToLower();

                var matches = from a in db.StoreAppInfos
                              where a.Name.ToLower().StartsWith(lowercaseTerm)
                              select a;

                if (matches.Count() >= count)
                {
                    return matches.Take(count).ToList();
                }

                var containsMatches = from a in db.StoreAppInfos
                                      where a.Name.ToLower().Contains(lowercaseTerm)
                                      select a;

                matches.ToList().AddRange(containsMatches);

                return matches.Count() <= count ? matches.ToList() : matches.Take(count).ToList();
            }
        }

        public static App AddNewAppFromStoreInfo(string msAppId, User user)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                StoreAppInfo appInfo = db.StoreAppInfos.Single(i => i.MsAppId == msAppId);
                AppRewardPlan rewardPlan = db.AppRewardPlans.Single(p => p.Type == AppRewardPlanType.None);
                App app = new App()
                    {
                        Name = appInfo.Name,
                        Copyright = appInfo.Copyright,
                        ShortDescription = appInfo.DescriptionHtml,
                        Description = appInfo.DescriptionHtml,
                        LogoLink50 = appInfo.LogoLink,
                        PackageFamilyName = appInfo.PackageFamilyName,
                        Platform = "Windows 8",
                        Publisher = appInfo.Developer,
                        RewardPlan = rewardPlan,
                        AppStoreLink = appInfo.AppStoreLink,
                        UserId = user.Id,
                        IsActive = true
                    };
                app.ComputeVerificationCode();
                App addedApp = db.Apps.Add(app);
                db.SaveChanges();
                user.Apps.Add(addedApp);
                db.SaveChanges();
                CacheOperations.User.Clear(user);
                return addedApp;
            }
        }

        public static void AddRecommendationPageView(AppAuthorization auth, RecommendationPage page,
                                                     bool isAutoOpen = false)
        {
            using (ReferEngineDatabaseContext db = new ReferEngineDatabaseContext())
            {
                RecommendationPageView pageView = new RecommendationPageView
                                                      {
                                                          AppReceiptId = auth.AppReceipt.Id,
                                                          TimeStamp = DateTime.UtcNow,
                                                          RecommendationPage = page,
                                                          AppId = auth.App.Id,
                                                          IsAutoOpen = isAutoOpen,
                                                          IpAddress = auth.UserHostAddress
                                                      };
                db.RecommendationPageViews.Add(pageView);
                db.SaveChanges();
            }
        }

        public static IpAddressLocation GetIpAddressLocation(string ipAddress)
        {
            var loc = CacheOperations.GetIpAddressLocation(ipAddress);
            if (loc == null)
            {
                using (var db = new ReferEngineDatabaseContext())
                {
                    loc =
                        db.IpAddressLocations.SingleOrDefault(l => l.IpAddress.Equals(ipAddress, StringComparison.OrdinalIgnoreCase));
                    if (loc == null)
                    {
                        if (ipAddress == "127.0.0.1")
                        {
                            loc =
                                db.IpAddressLocations.FirstOrDefault(l => l.City.Equals("Seattle", StringComparison.OrdinalIgnoreCase));
                        }
                        if (loc == null)
                        {
                            loc = IpCheckOperations.CheckIpAddress(ipAddress);
                            if (loc != null)
                            {
                                loc.IpAddress = ipAddress;
                                db.IpAddressLocations.Add(loc);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                CacheOperations.SetIpAddressLocation(loc);
            }
            return loc;
        }

        public static AppAuthorization GetAppAuthorization(string token)
        {
            AppAuthorization auth = CacheOperations.GetAppAuthorization(token);
            if (auth == null)
            {
                using (var db = new ReferEngineDatabaseContext())
                {
                    //auth = db.AppAuthorizations.Single(a => a.Token == token);
                    auth = db.AppAuthorizations.Where(a => a.Token == token)
                             .Include(a => a.App)
                             .Include(a => a.AppReceipt)
                             .First();
                    CacheOperations.AddAppAuthorization(auth, TimeSpan.FromHours(4));
                }
            }
            return auth;
        }

        public static void AddAppAuthorization(AppAuthorization auth)
        {
            using (var db = new ReferEngineDatabaseContext())
            {
                db.AppReceipts.Attach(auth.AppReceipt);
                db.Apps.Attach(auth.App);
                db.AppAuthorizations.Add(auth);
                CacheOperations.AddAppAuthorization(auth, TimeSpan.FromHours(4));
                db.SaveChanges();
            }

        }

        public static AppAutoShowOptions GetAppAutoShowOptions(long appId)
        {
            AppAutoShowOptions options = CacheOperations.GetAppAutoShowOptions(appId);
            if (options == null)
            {
                using (var db = new ReferEngineDatabaseContext())
                {
                    options = db.AppAutoShowOptions.Single(o => o.AppId == appId);
                    CacheOperations.SetAppAutoShowOptions(options);
                }
            }
            return options;
        }

        public static void AddFacebookPageView(FacebookPageView facebookPageView)
        {
            using (var db = new ReferEngineDatabaseContext())
            {
                db.FacebookPageViews.Add(facebookPageView);
                db.SaveChanges();
            }
        }
    }
}