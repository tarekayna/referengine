using System.Web;
using Itenso.TimePeriod;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using ReferEngine.Common.Properties;
using ReferEngine.Common.ViewModels.AppStore.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using ImageInfo = ReferEngine.Common.Models.ImageInfo;
using Membership = ReferEngine.Common.Models.Membership;

namespace ReferEngine.Common.Data
{
    internal static class DatabaseOperations
    {
        private static App QueryForActiveApp(DatabaseContext db, Expression<Func<App, bool>> expression, bool nullOk = false)
        {
            var q = db.Apps.Where(expression)
                           .Where(a => a.IsActive)
                           .Include(a => a.CloudinaryImages)
                           .Include(a => a.BackgroundImage)
                           .Include(a => a.HighQualityLogoImage)
                           .Include(a => a.LogoImage)
                           .Include(a => a.Category)
                           .Include(a => a.RewardPlan);
            return nullOk ? q.FirstOrDefault() : q.First();
        }

        private static List<App> QueryForActiveApps(DatabaseContext db, Expression<Func<App, bool>> expression)
        {
            return db.Apps.Where(expression)
                          .Where(a => a.IsActive)
                          .Include(a => a.CloudinaryImages)
                          .Include(a => a.BackgroundImage)
                          .Include(a => a.HighQualityLogoImage)
                          .Include(a => a.LogoImage)
                          .Include(a => a.Category)
                          .Include(a => a.RewardPlan)
                          .ToList();
        }

        private static WindowsAppStoreInfo QueryForWindowsAppStoreInfo(DatabaseContext db, Expression<Func<WindowsAppStoreInfo, bool>> expression, bool nullOk = false)
        {
            var q = db.WindowsAppStoreInfos.Where(expression)
                            .Include(i => i.CloudinaryImages)
                            .Include(i => i.LogoImage)
                            .Include(i => i.Category);
            return nullOk ? q.FirstOrDefault() : q.First();
        }

        private static IList<WindowsAppStoreInfo> QueryForWindowsAppStoreInfos(DatabaseContext db, Expression<Func<WindowsAppStoreInfo, bool>> expression)
        {
            return db.WindowsAppStoreInfos.Where(expression)
                     .Include(i => i.CloudinaryImages)
                     .Include(i => i.LogoImage)
                     .Include(i => i.Category)
                     .ToList();
        }

        internal static App GetApp(long id)
        {
            return (App)DbConnector.Execute(db => QueryForActiveApp(db, a => a.Id == id));
        }

        internal static App GetAppByName(string platform, string name)
        {
            return (App) DbConnector.Execute(db => QueryForActiveApp(db, a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              a.Platform.Equals(platform, StringComparison.InvariantCultureIgnoreCase)));
        }

        internal static App GetApp(string packageFamilyName, string appVerificationCode)
        {
            return (App)DbConnector.Execute(db => QueryForActiveApp(db, a => a.PackageFamilyName == packageFamilyName &&
                                                  a.VerificationCode == appVerificationCode));
        }

        internal static AppReceipt GetAppReceipt(string id)
        {
            return (AppReceipt)DbConnector.Execute(db => db.AppReceipts.SingleOrDefault(r => r.Id.Equals(id, StringComparison.OrdinalIgnoreCase)));
        }

        internal static Person GetPerson(Int64 facebookId)
        {
            return (Person) DbConnector.Execute(db => db.People.FirstOrDefault(p => p.FacebookId == facebookId));
        }
        
        internal static AppRecommendation GetAppRecommdation(long appId, long personFacebookId)
        {
            return
                (AppRecommendation)
                DbConnector.Execute(
                    db =>
                    db.AppRecommendations.FirstOrDefault(r => r.AppId == appId && r.PersonFacebookId == personFacebookId));
        }

        internal static IList<PrivateBetaSignup> GetPrivateBetaSignups()
        {
            return (IList<PrivateBetaSignup>)DbConnector.Execute(db => db.PrivateBetaSignups.OrderByDescending(i => i.RegistrationDateTime).ToList());
        }

        internal static IList<WindowsAppStoreLink> GetWindowsAppStoreLinks()
        {
            return (IList<WindowsAppStoreLink>) DbConnector.Execute(db => db.WindowsAppStoreLinks.ToList());
        }

        internal static ConfirmationCodeModel GetConfirmationCodeModel(string email)
        {
            return (ConfirmationCodeModel)DbConnector.Execute(db =>
            {
                var tokens = from m in db.Memberships
                                join u in db.Users on m.UserId equals u.Id
                                select new { u.Email, u.FirstName, m.ConfirmationToken };
                var token = tokens.FirstOrDefault(i => i.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (token == null) return null;
                var confirmationCodeModel = new ConfirmationCodeModel
                {
                    Email = token.Email,
                    ConfirmationCode = token.ConfirmationToken,
                    FirstName = token.FirstName
                };
                return confirmationCodeModel;                    
            });
        }

        internal static User GetUser(string email)
        {
            return (User) DbConnector.Execute(db =>
            {
                User user = db.Users.FirstOrDefault(c => c.Email == email);
                if (user == null) return null;
                user.Apps = QueryForActiveApps(db, a => a.UserId == user.Id);
                return user;
            });
        }

        internal static User GetUser(int id)
        {
            return (User) DbConnector.Execute(db =>
                {
                    User user = db.Users.First(c => c.Id == id);
                    user.Apps = QueryForActiveApps(db, a => a.UserId == user.Id);
                    return user;
                });
        }

        internal static User GetUserFromConfirmationCode(string code)
        {
            return (User) DbConnector.Execute(db =>
                {
                    Membership membership =
                        db.Memberships.First(m => m.ConfirmationToken.Equals(code, StringComparison.OrdinalIgnoreCase));
                    User user = db.Users.First(u => u.Id == membership.UserId);
                    user.Apps = QueryForActiveApps(db, a => a.UserId == user.Id);
                    return user;
                });
        }

        internal static Membership GetMembership(string email)
        {
            return (Membership) DbConnector.Execute(db =>
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
                });
        }

        internal static string GetRole(int userId)
        {
            return (string) DbConnector.Execute(db =>
                {
                    var userInRole = db.UsersInRoles.First(u => u.UserId == userId);
                    var role = db.Roles.First(r => r.RoleId == userInRole.RoleId);
                    return role.RoleName;
                });
        }

        internal static IList<UserMembership> GetUserMemberships()
        {
            return (IList<UserMembership>) DbConnector.Execute(db =>
                {
                    IList<UserMembership> result = new List<UserMembership>();
                    foreach (var user in db.Users)
                    {
                        var membership = db.Memberships.First(m => m.UserId == user.Id);
                        string role = GetRole(user.Id);
                        result.Add(new UserMembership(user, membership, role));
                    }
                    return result;
                });
        }

        internal static AppRecommendation GetAppRecommendation(long facebookPostId)
        {
            return (AppRecommendation) DbConnector.Execute(db => db.AppRecommendations.FirstOrDefault(r => r.FacebookPostId == facebookPostId));
        }

        internal static AppRecommendation GetAppRecommendation(long appId, long facebookId)
        {
            return (AppRecommendation)DbConnector.Execute(db => db.AppRecommendations.FirstOrDefault(r => r.AppId == appId && r.PersonFacebookId == facebookId));
        }

        internal static IList<AppRecommendation> GetAppRecommendations(long appId, int count = -1)
        {
            return (IList<AppRecommendation>) DbConnector.Execute(db =>
                {
                    var result = from r in db.AppRecommendations
                                 where r.AppId == appId
                                 orderby r.DateTime descending
                                 select r;

                    return count > -1 ? result.Take(count).ToList() : result.ToList();
                });
        }

        internal static IList<PersonRecommendationUnitResult> GetAppRecommendationsPeople(App app, int count, int start, DatabaseContext dbContext = null)
        {
            if (dbContext == null)
            {
                return (IList<PersonRecommendationUnitResult>)DbConnector.Execute(db =>
                {
                    var result = new List<PersonRecommendationUnitResult>();
                    var allRecommendations = from r in db.AppRecommendations
                                             where app.Id == r.AppId
                                             orderby r.DateTime descending
                                             select r;
                    var recommendations = allRecommendations.Skip(start - 1).Take(count);
                    foreach (AppRecommendation appRecommendation in recommendations)
                    {
                        var person = GetPerson(appRecommendation.PersonFacebookId) ?? new Person(true);
                        var location = appRecommendation.IpAddress == null ? null : GetIpAddressLocation(appRecommendation.IpAddress);
                        var res = new PersonRecommendationUnitResult(person, location, appRecommendation);
                        result.Add(res);
                    }
                    return result;
                });    
            }
            else
            {
                var db = dbContext;
                var result = new List<PersonRecommendationUnitResult>();
                var allRecommendations = from r in db.AppRecommendations
                                            where app.Id == r.AppId
                                            orderby r.DateTime descending
                                            select r;
                var recommendations = allRecommendations.Skip(start - 1).Take(count);
                foreach (AppRecommendation appRecommendation in recommendations)
                {
                    var person = GetPerson(appRecommendation.PersonFacebookId) ?? new Person(true);
                    var location = appRecommendation.IpAddress == null ? null : GetIpAddressLocation(appRecommendation.IpAddress);
                    var res = new PersonRecommendationUnitResult(person, location, appRecommendation);
                    result.Add(res);
                }
                return result;
            }
        }

        internal static IList<PersonRecommendationUnitResult> GetAppRecommendationsPeople(App app, TimeRange timeRange, string who)
        {
            return (IList<PersonRecommendationUnitResult>) DbConnector.Execute(db =>
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
                                var location = appRecommendation.IpAddress == null
                                                   ? null
                                                   : GetIpAddressLocation(appRecommendation.IpAddress);
                                var res = new PersonRecommendationUnitResult(person, location, appRecommendation);
                                result.Add(res);
                            }
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    return result;
                });
        }

        internal static int GetNumberOfFriends(Person person)
        {
            return (int) DbConnector.Execute(db => db.Friendships.Count(f => f.Person1FacebookId == person.FacebookId));
        }

        internal static List<ChartUnitResult> GetAppActionCount(App app, TimeRange timeRange, TimeSpan timeSpan, string who)
        {
            return (List<ChartUnitResult>) DbConnector.Execute(db =>
                {
                    var result = new List<ChartUnitResult>();

                    TimeRange currentRange = new TimeRange(timeRange.Start, timeRange.Start.Add((timeSpan)));
                    while (!currentRange.HasInside(timeRange.End))
                    {
                        DateTime rangeStart = currentRange.Start;
                        DateTime rangeEnd = currentRange.End;

                        int count;
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
                });
        }

        internal static List<MapUnitResult> GetAppActionLocations(App app, TimeRange timeRange, string who)
        {
            return (List<MapUnitResult>) DbConnector.Execute(db =>
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
                                                select
                                                    new
                                                        {
                                                            crc.Key.Country,
                                                            crc.Key.Region,
                                                            crc.Key.City,
                                                            Count = crc.Count()
                                                        };

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
                                                select
                                                    new
                                                        {
                                                            crc.Key.Country,
                                                            crc.Key.Region,
                                                            crc.Key.City,
                                                            Count = crc.Count()
                                                        };

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
                                                select
                                                    new
                                                        {
                                                            crc.Key.Country,
                                                            crc.Key.Region,
                                                            crc.Key.City,
                                                            Count = crc.Count()
                                                        };

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
                });
        }

        internal static WindowsAppStoreInfo GetWindowsAppStoreInfo(string msAppId)
        {
            return (WindowsAppStoreInfo)DbConnector.Execute(db => QueryForWindowsAppStoreInfo(db, i => i.MsAppId == msAppId, nullOk: true));
        }

        internal static WindowsAppViewModel GetWindowsAppViewModelByName(string appName)
        {
            return (WindowsAppViewModel)DbConnector.Execute(db =>
                {
                    string name = HttpUtility.HtmlEncode(appName);
                WindowsAppViewModel viewModel = new WindowsAppViewModel();
                viewModel.WindowsAppStoreInfo = QueryForWindowsAppStoreInfo(db, i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase), nullOk: true);
                if (viewModel.WindowsAppStoreInfo == null) return null;
                viewModel.App = QueryForActiveApp(db, a => a.PackageFamilyName == viewModel.WindowsAppStoreInfo.PackageFamilyName, nullOk: true);
                if (viewModel.App != null)
                {
                    var recommendations = from r in db.AppRecommendations
                                          where r.AppId == viewModel.App.Id
                                          orderby r.DateTime descending
                                          select r;
                    viewModel.NumberOfRecommendations = recommendations.Count();
                }
                return viewModel;
            });
        }

        internal static WindowsCategoryViewModel GetWindowsCategoryViewModel(string name, int numberOfApps, int pageNumber)
        {
            return (WindowsCategoryViewModel)DbConnector.Execute(db =>
            {
                WindowsCategoryViewModel viewModel = new WindowsCategoryViewModel();
                viewModel.Category = db.WindowsAppStoreCategories.FirstOrDefault(c => c.Name == name && string.IsNullOrEmpty(c.ParentCategoryName));
                if (viewModel.Category != null)
                {
                    viewModel.SubCategories =
                        db.WindowsAppStoreCategories.Where(c => c.ParentCategoryName == viewModel.Category.Name)
                          .ToList();

                    viewModel.WindowsAppStoreInfos = GetWindowsAppStoreInfosImpl(null, viewModel.Category.Name,
                                                                                 viewModel.Category.ParentCategoryName,
                                                                                 1,
                                                                                 18, db);
                }

                return viewModel;
            });
        }

        internal static IList<WindowsAppStoreInfo> GetWindowsAppStoreInfosImpl(string searchTerm, string category,
                                                                           string parentCategory, int page,
                                                                           int numberOfApps, DatabaseContext db)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowercaseTerm = searchTerm.ToLower();

                var matches = QueryForWindowsAppStoreInfos(db, i => i.Name.ToLower().StartsWith(lowercaseTerm));
                if (matches.Count() >= numberOfApps)
                {
                    return matches.Take(numberOfApps).ToList();
                }

                var containsMatches = QueryForWindowsAppStoreInfos(db, i => i.Name.ToLower().Contains(lowercaseTerm));
                matches.ToList().AddRange(containsMatches);
                return matches.Count() <= numberOfApps ? matches.ToList() : matches.Take(numberOfApps).ToList();
            }
            else
            {
                WindowsAppStoreCategory windowsAppStoreCategory;
                if (string.IsNullOrEmpty(parentCategory))
                {
                    windowsAppStoreCategory = db.WindowsAppStoreCategories.Single(c => string.IsNullOrEmpty(c.ParentCategoryName) &&
                                                                        c.Name.Equals(category, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    windowsAppStoreCategory = db.WindowsAppStoreCategories.Single(c => c.ParentCategoryName.Equals(parentCategory, StringComparison.InvariantCultureIgnoreCase)
                        && c.Name.Equals(category, StringComparison.InvariantCultureIgnoreCase));
                }
                int take = numberOfApps;
                int skip = numberOfApps * (page - 1);
                return QueryForWindowsAppStoreInfos(db, a => a.Category.Id == windowsAppStoreCategory.Id)
                                            .OrderByDescending(a => a.Rating)
                                            .Skip(skip)
                                            .Take(take)
                                            .ToList();
            }
            
        }

        internal static IList<WindowsAppStoreInfo> GetWindowsAppStoreInfos(string searchTerm, string category, string parentCategory, int page, int numberOfApps)
        {
            return (IList<WindowsAppStoreInfo>) DbConnector.Execute(db => GetWindowsAppStoreInfosImpl(searchTerm, category, parentCategory, page, numberOfApps, db));
        }

        internal static IpAddressLocation GetIpAddressLocation(string ipAddress)
        {
            return (IpAddressLocation) DbConnector.Execute(db =>
                {
                    IpAddressLocation location =
                        db.IpAddressLocations.SingleOrDefault(
                            l => l.IpAddress.Equals(ipAddress, StringComparison.OrdinalIgnoreCase));
                    if (location == null)
                    {
                        if (ipAddress == "127.0.0.1")
                        {
                            location =
                                db.IpAddressLocations.FirstOrDefault(
                                    l => l.City.Equals("Seattle", StringComparison.OrdinalIgnoreCase));
                        }
                        if (location == null)
                        {
                            location = IpCheckOperations.CheckIpAddress(ipAddress);
                            if (location != null)
                            {
                                location.IpAddress = ipAddress;
                                try
                                {
                                    db.IpAddressLocations.Add(location);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Trace.TraceError(e.Message);
                                }
                            }
                        }
                    }
                    return location;
                });
        }

        internal static AppAuthorization GetAppAuthorization(string token)
        {
            return (AppAuthorization)DbConnector.Execute(db =>
                {
                    AppAuthorization auth = db.AppAuthorizations.Where(a => a.Token == token)
                                              .Include(a => a.AppReceipt)
                                              .Include(a => a.App)
                                              .First();
                    auth.App = QueryForActiveApp(db, a => a.Id == auth.App.Id);
                    return auth;
                });
        }

        internal static AppAutoShowOptions GetAppAutoShowOptions(long appId)
        {
            return (AppAutoShowOptions)DbConnector.Execute(db => db.AppAutoShowOptions.Single(o => o.AppId == appId));
        }

        internal static Invite GetInvite(string email)
        {
            return (Invite) DbConnector.Execute(db => db.Invites.FirstOrDefault(i => i.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)));
        }

        internal static WindowsAppStoreCategory GetWindowsAppStoreCategory(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return (WindowsAppStoreCategory)DbConnector.Execute(db => db.WindowsAppStoreCategories.SingleOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        internal static IList<WindowsAppStoreCategory> GetWindowsAppStoreCategories()
        {
            return (IList<WindowsAppStoreCategory>)DbConnector.Execute(db => db.WindowsAppStoreCategories.Where(c => string.IsNullOrEmpty(c.ParentCategoryName)).ToList());
        }

        internal static IList<WindowsAppStoreCategory> GetWindowsAppStoreSubCategories(int parentCategoryId)
        {
            return (IList<WindowsAppStoreCategory>)DbConnector.Execute(db =>
            {
                var parentCategory = db.WindowsAppStoreCategories.SingleOrDefault(c => c.Id == parentCategoryId);
                return parentCategory != null ? db.WindowsAppStoreCategories.Where(c => c.ParentCategoryName == parentCategory.Name).ToList() : null;
            });
        }

        internal static App SetAppAsInActive(App app)
        {
            return (App) DbConnector.Execute(db =>
                {
                    db.Apps.Attach(app);
                    app.IsActive = false;
                    db.SaveChanges();
                    return app;
                });
        }

        internal static void AddOrUpdateAppReceipt(AppReceipt appReceipt)
        {
            DbConnector.Execute(db =>
                {
                    AppReceipt receipt = db.AppReceipts.FirstOrDefault(r => r.Id == appReceipt.Id);
                    if (receipt == null)
                    {
                        db.AppReceipts.Add(appReceipt);
                    }
                    else
                    {
                        receipt.PersonFacebookId = appReceipt.PersonFacebookId;
                    }

                    db.SaveChanges();
                    return null;
                });
        }

        internal static void AddApp(App app)
        {
            DbConnector.Execute(db =>
            {
                db.Apps.Add(app);
                db.SaveChanges();
                return null;
            });
        }

        internal static void AddAppReceipt(AppReceipt receipt)
        {
            DbConnector.Execute(db =>
            {
                if (!db.AppReceipts.Any(r => r.Id == receipt.Id))
                {
                    db.AppReceipts.Add(receipt);
                    db.SaveChanges();
                }
                return null;
            });
        }

        internal static void AddOrUpdatePerson(Person person)
        {
            DbConnector.Execute(db =>
                {
                    AddOrUpdatePerson(person, db);
                    db.SaveChanges();
                    return null;
                });
        }

        private static void AddOrUpdatePerson(Person person, DatabaseContext db)
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

        internal static void AddPersonAndFriends(Person person, IList<Person> friends, BrokeredMessage message)
        {
            DbConnector.Execute(db =>
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
                    return null;
                });
        }

        internal static void AddRecommendation(AppRecommendation recommendation)
        {
            DbConnector.Execute(db =>
                {
                    var existing =
                        db.AppRecommendations.FirstOrDefault(r => r.FacebookPostId == recommendation.FacebookPostId);
                    if (existing != null) return null;
                    db.AppRecommendations.Add(recommendation);
                    db.SaveChanges();
                    return null;
                });
        }

        internal static void AddPrivateBetaSignup(PrivateBetaSignup privateBetaSignup)
        {
            DbConnector.Execute(db =>
                {
                    if (!db.PrivateBetaSignups.Any(s => s.Email == privateBetaSignup.Email))
                    {
                        db.PrivateBetaSignups.Add(privateBetaSignup);
                        db.SaveChanges();
                    }
                    return null;
                });
        }

        internal static void AddWindowsAppStoreLink(WindowsAppStoreLink appWebLink)
        {
            DbConnector.Execute(db =>
                {
                    if (!db.WindowsAppStoreLinks.Any(l => l.Link == appWebLink.Link))
                    {
                        try
                        {
                            db.WindowsAppStoreLinks.Add(appWebLink);
                            db.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            Trace.TraceError(e.Message);
                        }
                    }
                    return null;
                });
        }

        internal static void AddOrUpdateWindowsAppStoreLinks(IList<WindowsAppStoreLink> windowsAppStoreLinks)
        {
            DbConnector.Execute(db =>
                {
                    const int take = 1000;
                    int skip = 0;

                    while (true)
                    {
                        var currentSet = windowsAppStoreLinks.Skip(skip).Take(take);
                        skip += take;
                        foreach (var appWebLink in currentSet)
                        {
                            try
                            {
                                WindowsAppStoreLink existing =
                                    db.WindowsAppStoreLinks.SingleOrDefault(l => l.Link == appWebLink.Link);
                                if (existing == null)
                                {
                                    db.WindowsAppStoreLinks.Add(appWebLink);
                                }
                                else
                                {
                                    existing.LastUpdated = DateTime.UtcNow;
                                }
                            }
                            catch (EntityCommandExecutionException e)
                            {
                                Trace.TraceError(e.Message);
                            }
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            Trace.TraceError(e.Message);
                        }

                        if (currentSet.Count() < take)
                        {
                            break;
                        }
                    }
                    return null;
                });
        }

        internal static void UpdateWindowsAppStoreLink(WindowsAppStoreLink windowsAppStoreLink)
        {
            DbConnector.Execute(db =>
            {
                db.WindowsAppStoreLinks.Attach(windowsAppStoreLink);
                db.SaveChanges();
                return null;
            });
        }

        internal static void DeleteWindowsAppStoreLink(WindowsAppStoreLink windowsAppStoreLink)
        {
            DbConnector.Execute(db =>
            {
                db.WindowsAppStoreLinks.Attach(windowsAppStoreLink);
                db.WindowsAppStoreLinks.Remove(windowsAppStoreLink);
                db.SaveChanges();
                return null;
            });
            
        }

        internal static bool AddOrUpdateWindowsAppStoreInfo(WindowsAppStoreInfo storeAppInfo, string categoryString, string logoLink, IList<ImageInfo> images)
        {
            return (bool) DbConnector.Execute(db =>
            {
                string[] categories = categoryString.Replace("&amp;", "&").Trim().Split('/');
                int numberOfCategories = categories.Count();
                if (numberOfCategories != 1 || numberOfCategories != 2)
                {
                    throw new InvalidOperationException("Number of categories: " + numberOfCategories);
                }
                string categoryName = categories.Last().Trim();
                string parentyCategoryName = numberOfCategories == 2 ? categories.First().Trim() : null;
                WindowsAppStoreCategory category;
                if (string.IsNullOrEmpty(parentyCategoryName))
                {
                    category = db.WindowsAppStoreCategories.SingleOrDefault(c => c.Name.Equals(categoryName, StringComparison.InvariantCultureIgnoreCase)
                                                                   && string.IsNullOrEmpty(c.ParentCategoryName));
                }
                else
                {
                    category = db.WindowsAppStoreCategories.SingleOrDefault(c => c.Name.Equals(categoryName, StringComparison.InvariantCultureIgnoreCase)
                                                                   && c.ParentCategoryName.Equals(parentyCategoryName, StringComparison.InvariantCultureIgnoreCase));
                }
                if (category == null)
                {
                    category = new WindowsAppStoreCategory
                    {
                        Name = categoryName,
                        ParentCategoryName = parentyCategoryName
                    };
                }
                storeAppInfo.Category = category;
                
                var existing = QueryForWindowsAppStoreInfo(db, i => i.MsAppId.Equals(storeAppInfo.MsAppId), nullOk: true);
                if (existing != null)
                {
                    var newImages = images.Where(image => existing.CloudinaryImages.All(c => c.OriginalLink != image.Link));
                    var removedImages = existing.CloudinaryImages.Where(cloudinaryImage => images.All(i => i.Link != cloudinaryImage.OriginalLink));

                    var removedImagesList = removedImages.ToList();
                    foreach (CloudinaryImage cloudinaryImage in removedImagesList)
                    {
                        existing.CloudinaryImages.Remove(cloudinaryImage);
                        CloudinaryConnector.DeleteImage(cloudinaryImage);
                        db.CloudinaryImages.Remove(cloudinaryImage);
                    }

                    var newImagesList = newImages.ToList();
                    foreach (ImageInfo newImage in newImagesList)
                    {
                        CloudinaryImage cloudinaryImage = CloudinaryConnector.UploadRemoteImage(newImage);
                        existing.CloudinaryImages.Add(cloudinaryImage);
                    }

                    if (existing.LogoImage == null)
                    {
                        CloudinaryImage logoImage = CloudinaryConnector.UploadRemoteImage(new ImageInfo { Link = logoLink });
                        existing.LogoImage = logoImage;
                        
                    }
                    else if (logoLink != existing.LogoImage.OriginalLink)
                    {
                        CloudinaryConnector.DeleteImage(existing.LogoImage);
                        db.CloudinaryImages.Remove(existing.LogoImage);

                        CloudinaryImage logoImage = CloudinaryConnector.UploadRemoteImage(new ImageInfo { Link = logoLink });
                        existing.LogoImage = logoImage;
                    }

                    existing.Update(storeAppInfo);
                    db.SaveChanges();
                    return false;
                }
                else
                {
                    foreach (ImageInfo imageInfo in images)
                    {
                        CloudinaryImage cloudinaryImage = CloudinaryConnector.UploadRemoteImage(imageInfo);
                        storeAppInfo.CloudinaryImages.Add(cloudinaryImage);
                    }
                    CloudinaryImage logoImage = CloudinaryConnector.UploadRemoteImage(new ImageInfo {Link = logoLink});
                    storeAppInfo.LogoImage = logoImage;
                    db.WindowsAppStoreInfos.Add(storeAppInfo);
                    db.SaveChanges();
                    return true;
                }
            });
        }

        internal static void AddUserRole(User user, string roleName)
        {
            DbConnector.Execute(db =>
                {
                    Role role = db.Roles.First(r => r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));
                    db.UsersInRoles.Add(new UserInRole {RoleId = role.RoleId, UserId = user.Id});
                    db.SaveChanges();
                    return null;
                });
        }

        internal static App AddNewAppFromStoreInfo(string msAppId, User user)
        {
            return (App) DbConnector.Execute(db =>
                {
                    WindowsAppStoreInfo appInfo = QueryForWindowsAppStoreInfo(db, i => i.MsAppId == msAppId);
                    AppRewardPlan rewardPlan = db.AppRewardPlans.Single(p => p.Type == AppRewardPlanType.None);
                    App app = new App
                        {
                            Name = appInfo.Name,
                            Copyright = appInfo.Copyright,
                            ShortDescription = appInfo.DescriptionHtml,
                            Description = appInfo.DescriptionHtml,
                            LogoImage = appInfo.LogoImage,
                            PackageFamilyName = appInfo.PackageFamilyName,
                            Platform = "Windows",
                            Publisher = appInfo.Developer,
                            Category =  appInfo.Category,
                            RewardPlan = rewardPlan,
                            AppStoreLink = appInfo.AppStoreLink,
                            UserId = user.Id,
                            IsActive = true,
                            CloudinaryImages = appInfo.CloudinaryImages,
                            BackgroundColor = appInfo.BackgroundColor,
                            BackgroundImage = new CloudinaryImage { Id = "app_default_background_pjr3qo", Format = "jpg" }
                        };
                    app.ComputeVerificationCode();
                    App addedApp = db.Apps.Add(app);
                    db.SaveChanges();
                    user.Apps.Add(addedApp);
                    db.SaveChanges();

                    CacheOperations.User.Remove(user);
                    return addedApp;
                });
        }

        internal static void AddRecommendationPageView(AppAuthorization auth, RecommendationPage page,
                                                     bool isAutoOpen = false)
        {
            DbConnector.Execute(db =>
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
                    return null;
                });
        }

        internal static void AddAppAuthorization(AppAuthorization auth)
        {
            DbConnector.Execute(db =>
                {
                    db.AppReceipts.Attach(auth.AppReceipt);
                    db.Apps.Attach(auth.App);
                    db.AppAuthorizations.Add(auth);
                    CacheOperations.AddAppAuthorization(auth, TimeSpan.FromHours(4));
                    db.SaveChanges();
                    return null;
                });
        }

        internal static void AddFacebookPageView(FacebookPageView facebookPageView)
        {
            DbConnector.Execute(db =>
                {
                    db.FacebookPageViews.Add(facebookPageView);
                    db.SaveChanges();
                    return null;
                });
        }

        internal static void AddInvite(Invite invite)
        {
            DbConnector.Execute(db =>
                {
                    db.Invites.Add(invite);
                    db.SaveChanges();
                    return null;
                });
        }
    }
}
