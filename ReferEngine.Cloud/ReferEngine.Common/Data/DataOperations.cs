using Itenso.TimePeriod;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ReferEngine.Common.ViewModels.AppStore.Windows;
using Membership = ReferEngine.Common.Models.Membership;

namespace ReferEngine.Common.Data
{
    public static class DataOperations
    {
        public static App GetApp(long id)
        {
            App app = CacheOperations.AppById.Get(id);
            if (app == null)
            {
                app = DatabaseOperations.GetApp(id);
                CacheOperations.AppById.Add(app);
            }
            return app;
        }

        public static App GetAppByName(string platform, string name)
        {
            App app = CacheOperations.AppByPlatformAndName.Get(platform, name);
            if (app == null)
            {
                app = DatabaseOperations.GetAppByName(platform, name);
                CacheOperations.AppByPlatformAndName.Add(app);
            }
            return app;
        }

        public static App GetApp(string packageFamilyName, string appVerificationCode)
        {
            App app = CacheOperations.AppByPackageAndVerification.Get(packageFamilyName, appVerificationCode);
            if (app == null)
            {
                app = DatabaseOperations.GetApp(packageFamilyName, appVerificationCode);
                CacheOperations.AppByPackageAndVerification.Add(app);
            }
            return app;
        }

        public static AppReceipt GetAppReceipt(string id)
        {
            return DatabaseOperations.GetAppReceipt(id);
        }

        public static FacebookOperations GetFacebookOperations(string token)
        {
            return CacheOperations.GetFacebookOperations(token);
        }

        public static Person GetPerson(Int64 facebookId)
        {
            Person person = CacheOperations.GetPerson(facebookId);
            if (person == null)
            {
                person = DatabaseOperations.GetPerson(facebookId);
                if (person == null)
                {
                    Trace.TraceWarning("GetPerson returned null: " + facebookId);
                }
                else
                {
                    CacheOperations.AddPerson(person);
                }
            }
            return person;
        }

        public static AppRecommendation GetAppRecommdation(long appId, long personFacebookId)
        {
            return DatabaseOperations.GetAppRecommdation(appId, personFacebookId);
        }

        public static IList<PrivateBetaSignup> GetPrivateBetaSignups()
        {
            return DatabaseOperations.GetPrivateBetaSignups();
        }

        public static IList<WindowsAppStoreLink> GetWindowsAppStoreLinks()
        {
            return DatabaseOperations.GetWindowsAppStoreLinks();
        }

        public static ConfirmationCodeModel GetConfirmationCodeModel(string email)
        {
            return DatabaseOperations.GetConfirmationCodeModel(email);
        }
        
        public static User GetUser(string email)
        {
            return DatabaseOperations.GetUser(email);
        }

        public static User GetUser(int id)
        {
            User user = CacheOperations.User.Get(id);
            if (user == null)
            {
                user = DatabaseOperations.GetUser(id);
                CacheOperations.User.Add(user);
            }
            return user;
        }

        public static User GetUserFromConfirmationCode(string code)
        {
            return DatabaseOperations.GetUserFromConfirmationCode(code);
        }

        public static Membership GetMembership(string email)
        {
            return DatabaseOperations.GetMembership(email);
        }

        public static string GetRole(int userId)
        {
            return DatabaseOperations.GetRole(userId);
        }

        public static IList<UserMembership> GetUserMemberships()
        {
            return DatabaseOperations.GetUserMemberships();
        }

        public static AppRecommendation GetAppRecommendation(long facebookPostId)
        {
            return DatabaseOperations.GetAppRecommendation(facebookPostId);
        }

        public static AppRecommendation GetAppRecommendation(long appId, long facebookId)
        {
            return DatabaseOperations.GetAppRecommendation(appId, facebookId);
        }

        public static IList<AppRecommendation> GetAppRecommendations(long appId, int count = -1)
        {
            return DatabaseOperations.GetAppRecommendations(appId, count);
        }

        public static IList<PersonRecommendationUnitResult> GetAppRecommendationsPeople(App app, TimeRange timeRange, string who)
        {
            return DatabaseOperations.GetAppRecommendationsPeople(app, timeRange, who);
        }

        public static int GetNumberOfFriends(Person person)
        {
            return DatabaseOperations.GetNumberOfFriends(person);
        }

        public static List<ChartUnitResult> GetAppActionCount(App app, TimeRange timeRange, TimeSpan timeSpan, string who)
        {
            return DatabaseOperations.GetAppActionCount(app, timeRange, timeSpan, who);
        }

        public static List<MapUnitResult> GetAppActionLocations(App app, TimeRange timeRange, string who)
        {
            return DatabaseOperations.GetAppActionLocations(app, timeRange, who);
        }

        public static IList<WindowsAppStoreInfo> GetWindowsAppStoreInfos(string term, int count)
        {
            return DatabaseOperations.GetWindowsAppStoreInfos(term, count);
        }

        public static WindowsAppStoreInfo GetWindowsAppStoreInfo(string msAppId)
        {
            return DatabaseOperations.GetWindowsAppStoreInfo(msAppId);
        }

        public static WindowsAppViewModel GetWindowsAppViewModelByName(string name)
        {
            WindowsAppViewModel viewModel = CacheOperations.WindowsAppViewModelByName.Get(name);
            if (viewModel == null)
            {
                viewModel = DatabaseOperations.GetWindowsAppViewModelByName(name);
                if (viewModel != null)
                {
                    CacheOperations.WindowsAppViewModelByName.Add(viewModel);
                }
            }
            return viewModel;
        }

        public static WindowsCategoryViewModel GetWindowsCategoryViewModel(string name, int numberOfApps, int pageNumber)
        {
            WindowsCategoryViewModel viewModel = CacheOperations.WindowsCategoryViewModel.Get(name);
            if (viewModel == null)
            {
                viewModel = DatabaseOperations.GetWindowsCategoryViewModel(name, numberOfApps, pageNumber);
                if (viewModel != null)
                {
                    CacheOperations.WindowsCategoryViewModel.Add(viewModel);
                }
            }
            return viewModel;
        }

        public static IList<PersonRecommendationUnitResult> GetAppRecommendationsPeople(App app, int count, int start)
        {
            return DatabaseOperations.GetAppRecommendationsPeople(app, count, start);
        }

        public static IpAddressLocation GetIpAddressLocation(string ipAddress)
        {
            var location = CacheOperations.GetIpAddressLocation(ipAddress);
            if (location == null)
            {
                location = DatabaseOperations.GetIpAddressLocation(ipAddress);
                CacheOperations.SetIpAddressLocation(location);
            }
            return location;
        }

        public static AppAuthorization GetAppAuthorization(string token)
        {
            AppAuthorization auth = CacheOperations.GetAppAuthorization(token);
            if (auth == null)
            {
                auth = DatabaseOperations.GetAppAuthorization(token);
                CacheOperations.AddAppAuthorization(auth, TimeSpan.FromHours(4));
            }
            return auth;
        }

        public static AppAutoShowOptions GetAppAutoShowOptions(long appId)
        {
            AppAutoShowOptions options = CacheOperations.GetAppAutoShowOptions(appId);
            if (options == null)
            {
                options = DatabaseOperations.GetAppAutoShowOptions(appId);
                CacheOperations.SetAppAutoShowOptions(options);
            }
            return options;
        }

        public static Invite GetInvite(string email)
        {
            return DatabaseOperations.GetInvite(email);
        }        

        public static void SetAppAsInActive(App app)
        {
            DatabaseOperations.SetAppAsInActive(app);
            CacheOperations.User.Remove(GetUser(app.UserId));
        }

        public static void AddOrUpdateAppReceipt(AppReceipt appReceipt)
        {
            DatabaseOperations.AddOrUpdateAppReceipt(appReceipt);
        }

        public static void AddApp(App app)
        {
            DatabaseOperations.AddApp(app);
        }

        public static void AddAppReceipt(AppReceipt receipt)
        {
            DatabaseOperations.AddAppReceipt(receipt);
        }

        public static void AddOrUpdatePerson(Person person)
        {
            DatabaseOperations.AddOrUpdatePerson(person);
        }

        public static void AddPersonAndFriends(Person person, IList<Person> friends, BrokeredMessage message)
        {
            DatabaseOperations.AddPersonAndFriends(person, friends, message);
        }

        public static void AddRecommendation(AppRecommendation recommendation)
        {
            DatabaseOperations.AddRecommendation(recommendation);
        }

        public static void AddPrivateBetaSignup(PrivateBetaSignup privateBetaSignup)
        {
            DatabaseOperations.AddPrivateBetaSignup(privateBetaSignup);
        }

        public static void AddWindowsAppStoreLink(WindowsAppStoreLink appWebLink)
        {
            DatabaseOperations.AddWindowsAppStoreLink(appWebLink);
        }

        public static void AddOrUpdateWindowsAppStoreLinks(IList<WindowsAppStoreLink> windowsAppStoreLinks)
        {
            DatabaseOperations.AddOrUpdateWindowsAppStoreLinks(windowsAppStoreLinks);
        }

        public static void UpdateWindowsAppStoreLink(WindowsAppStoreLink windowsAppStoreLink)
        {
            DatabaseOperations.UpdateWindowsAppStoreLink(windowsAppStoreLink);
        }

        public static void DeleteWindowsAppStoreLink(WindowsAppStoreLink windowsAppStoreLink)
        {
            DatabaseOperations.DeleteWindowsAppStoreLink(windowsAppStoreLink);
        }

        public static bool AddOrUpdateWindowsAppStoreInfo(WindowsAppStoreInfo storeAppInfo, string category, string logoLink, IList<ImageInfo> images)
        {
            return DatabaseOperations.AddOrUpdateWindowsAppStoreInfo(storeAppInfo, category, logoLink, images);
        }

        public static WindowsAppStoreCategory GetWindowsAppStoreCategory(string name)
        {
            return DatabaseOperations.GetWindowsAppStoreCategory(name);
        }

        public static IList<WindowsAppStoreCategory> GetWindowsAppStoreCategories()
        {
            return DatabaseOperations.GetWindowsAppStoreCategories();
        }

        public static void AddUserRole(User user, string roleName)
        {
            DatabaseOperations.AddUserRole(user, roleName);
        }

        public static App AddNewAppFromStoreInfo(string msAppId, User user)
        {
            return DatabaseOperations.AddNewAppFromStoreInfo(msAppId, user);
        }

        public static void AddRecommendationPageView(AppAuthorization auth, RecommendationPage page,
                                                     bool isAutoOpen = false)
        {
            DatabaseOperations.AddRecommendationPageView(auth, page, isAutoOpen);
        }

        public static void AddAppAuthorization(AppAuthorization auth)
        {
            CacheOperations.AddAppAuthorization(auth, TimeSpan.FromMinutes(20));
            DatabaseOperations.AddAppAuthorization(auth);
        }

        public static void AddFacebookOperations(FacebookOperations facebookOperations)
        {
            CacheOperations.AddFacebookOperations(facebookOperations.ReferEngineAuthToken, facebookOperations);
        }

        public static void AddFacebookPageView(FacebookPageView facebookPageView)
        {
            DatabaseOperations.AddFacebookPageView(facebookPageView);
        }

        public static void AddInvite(Invite invite)
        {
            DatabaseOperations.AddInvite(invite);
        }
    }
}