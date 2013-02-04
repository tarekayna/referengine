using System.Net;
using System.Threading;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ReferEngine.Web.Models.Common;
using ReferEngine.Web.Models.Recommend.Win8;
using StackExchange.Profiling;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    [OutputCache(Duration = 0)]
    public class RecommendController : BaseController
    {
        public RecommendController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public ActionResult Intro(string platform, long id)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(id, "id");

            ViewProperties.CurrentApp = DataReader.GetApp(id);
            string viewName = String.Format("{0}/intro", platform);
            return View(viewName, ViewProperties.CurrentApp);
        }

        public ActionResult RecommendViewOnly(string platform, long id)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(id, "id");

            ViewProperties.CurrentApp = DataReader.GetApp(id);

            // Facebook is cool
            Person me = DatabaseOperations.GetPerson(509572882);

            IList<Person> friends = new List<Person>();
            friends.Add(DatabaseOperations.GetPerson(12625308));
            friends.Add(DatabaseOperations.GetPerson(100002179707322));
            friends.Add(DatabaseOperations.GetPerson(1488075933));

            string viewName = String.Format("{0}/recommend", platform);
            var viewModel = new RecommendViewModel(me, ViewProperties.CurrentApp, "fake_auth_token", null);
            return View(viewName, viewModel);
        }

        public async Task<ActionResult> Recommend(string platform, string re_auth_token, string fb_access_code)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(re_auth_token, "re_auth_token");
            Verifier.IsNotNullOrEmpty(fb_access_code, "fb_access_code");

            var profiler = MiniProfiler.Current;

            using (profiler.Step("Recommend"))
            {
                bool showErrorView = false;
                AppAuthorization appAuthorization = GetAppAuthorization(re_auth_token, shouldBeVerified: false);
                Verifier.IsNotNullOrEmpty(appAuthorization, "appAuthorization");

                // App is cool
                ViewProperties.CurrentApp = appAuthorization.App;
                string accessCode = fb_access_code.Replace("%23", "#");
                FacebookOperations facebookOperations = await FacebookOperations.CreateAsync(accessCode, re_auth_token);

                // Facebook is cool
                Person me = await facebookOperations.GetCurrentUserAsync();

                AppReceipt appReceipt = appAuthorization.AppReceipt;
                AppReceipt existingReceipt = DataReader.GetAppReceipt(appReceipt.Id);
                if (existingReceipt != null && existingReceipt.PersonFacebookId.HasValue)
                {
                    // App + Person: something is up
                    if (existingReceipt.PersonFacebookId != me.FacebookId)
                    {
                        // Another user is associated with this receipt
                        AppRecommendation appRecommendation = DataReader.GetAppRecommendation(ViewProperties.CurrentApp.Id,
                                                                                              existingReceipt.PersonFacebookId.Value);
                        if (appRecommendation != null)
                        {
                            // Another user already posted a recommendation for this app using this receipt
                            showErrorView = true;
                        }
                        //else
                        //{
                              // Well that user logged in, but didn't submit a recommendation
                              // That's ok, continue
                        //}
                    }
                    //else
                    //{
                    //    // Current user is already associated with this receipt
                    //    // Two options:
                    //    //          1- Person has already posted a recommendation for this app using this receipt
                    //    //             It's fine, they can post again
                    //    //          2- Person has logged in before but did not submit a recommendation
                    //    //             That's ok, continue
                    //}
                }

                if (showErrorView)
                {
                    string viewName = String.Format("{0}/recommend-error", platform);
                    var viewModel = new RecommendViewModel(me, ViewProperties.CurrentApp, re_auth_token, appReceipt);
                    return View(viewName, viewModel);
                }
                else
                {
                    // AppReceipt should already be in the database
                    // Update it with the Facebook Id of the user
                    appReceipt.PersonFacebookId = me.FacebookId;
                    DataWriter.AddAppReceipt(appReceipt);

                    using (profiler.Step("DataWriter.AddFacebookOperations"))
                    {
                        DataWriter.AddFacebookOperations(re_auth_token, facebookOperations);
                    }

                    string viewName = String.Format("{0}/recommend", platform);
                    var viewModel = new RecommendViewModel(me, ViewProperties.CurrentApp, re_auth_token, appReceipt);
                    return View(viewName, viewModel);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostRecommendation(string platform, string re_auth_token, string message)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(re_auth_token, "re_auth_token");

            AppAuthorization appAuthorization = GetAppAuthorization(re_auth_token, shouldBeVerified: false);
            Verifier.IsNotNullOrEmpty(appAuthorization, "appAuthorization");

            var facebookOperations = DataReader.GetFacebookOperations(re_auth_token);
            Verifier.IsNotNullOrEmpty(facebookOperations, "facebookOperations");
            
            var me = await facebookOperations.GetCurrentUserAsync();
            var recommendation = new AppRecommendation
                                        {
                                            AppId = appAuthorization.App.Id,
                                            PersonFacebookId = me.FacebookId,
                                            UserMessage = message
                                        };

            var postedRecommendation = await facebookOperations.PostAppRecommendationAsync(recommendation);
            DataWriter.AddAppRecommendation(postedRecommendation);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<ActionResult> GetFriends(string platform, string re_auth_token)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(re_auth_token, "re_auth_token");

            AppAuthorization appAuthorization = GetAppAuthorization(re_auth_token, shouldBeVerified: false);
            Verifier.IsNotNullOrEmpty(appAuthorization, "appAuthorization");

            var facebookOperations = DataReader.GetFacebookOperations(re_auth_token);
            Verifier.IsNotNullOrEmpty(facebookOperations, "facebookOperations");

            var friends = await facebookOperations.GetFriendsAsync();

            // Add this here so we don't have to request the friends 
            // from the Worker. The token might have expired.
            ServiceBusOperations.AddToQueue(facebookOperations);
                    
            return Json(new {friends = Person.Serialize(friends)});
        }

        private AppAuthorization GetAppAuthorization(string referEngineAuthToken, bool shouldBeVerified)
        {
            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            AppAuthorization appAuthorization = DataReader.GetAppAuthorization(referEngineAuthToken, userHostAddress);
            const int maxTryCount = 40;
            for (int i = 0; i < maxTryCount; i++)
            {
                if ((appAuthorization != null) && (appAuthorization.IsVerified || !shouldBeVerified))
                {
                    break;
                }

                Thread.Sleep(500);
                appAuthorization = DataReader.GetAppAuthorization(referEngineAuthToken, userHostAddress);
            }

            if (appAuthorization != null && !appAuthorization.IsVerified && shouldBeVerified)
            {
                appAuthorization = null;
            }

            return appAuthorization;
        }
    }
}
