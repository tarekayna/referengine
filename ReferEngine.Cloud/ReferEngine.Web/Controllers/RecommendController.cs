using System.Threading;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ReferEngine.Web.Models.Refer.Win8;

namespace ReferEngine.Web.Controllers
{
    public class RecommendController : Controller
    {
        private IReferDataReader DataReader { get; set; }
        private IReferDataWriter DataWriter { get; set; }

        public RecommendController(IReferDataReader dataReader, IReferDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }

        [OutputCache(Duration=0)]
        [RequireHttps]
        public ActionResult Intro(string platform, long id)
        {
            App app = DataReader.GetApp(id);
            string viewName = String.Format("{0}/intro", platform);
            return View(viewName, app);
        }

        //TODO Remove this action
        [OutputCache(Duration = 0)]
        [RequireHttps]
        public async Task<ActionResult> RecommendViewOnly(string platform, long id)
        {
            App app = DataReader.GetApp(id);

            // Facebook is cool
            Person me = DatabaseOperations.GetPerson(509572882);

            IList<Person> friends = new List<Person>();
            friends.Add(DatabaseOperations.GetPerson(12625308));
            friends.Add(DatabaseOperations.GetPerson(100002179707322));
            friends.Add(DatabaseOperations.GetPerson(1488075933));
            CurrentUser currentUser = new CurrentUser
            {
                Person = me,
                Friends = friends
            };

            string viewName = String.Format("{0}/recommend", platform);
            var viewModel = new RecommendViewModel(currentUser, app, "fake_auth_token");
            return View(viewName, viewModel);
        }

        [OutputCache(Duration = 0)]
        [RequireHttps]
        public async Task<ActionResult> Recommend(string platform, string re_auth_token, string fb_access_code)
        {
            AppAuthorization appAuthorization = GetAppAuthorization(re_auth_token, false);
            if (appAuthorization != null)
            {
                // App is cool
                App app = appAuthorization.App;
                string accessCode = fb_access_code.Replace("%23", "#");
                FacebookOperations facebookOperations = await FacebookOperations.CreateAsync(accessCode);

                // Facebook is cool
                Person me = await facebookOperations.GetCurrentUserAsync();

                AppReceipt appReceipt = appAuthorization.AppReceipt;
                AppReceipt existingReceipt = DataReader.GetAppReceipt(appReceipt.Id);
                if (existingReceipt != null && existingReceipt.PersonFacebookId.HasValue)
                {
                    // App + Person: something is up
                    if (existingReceipt.PersonFacebookId == me.FacebookId)
                    {
                        // Current user is already associated with this receipt
                        AppRecommendation appRecommendation = DataReader.GetAppRecommendation(app.Id, me.FacebookId);
                        if (appRecommendation != null)
                        {
                            // Person has already posted a recommendation for this app using this receipt
                            // TODO tell the user they already recommended
                        }

                        // Person has logged in before but did not submit a recommendation
                        // That's ok, continue
                    }
                    else
                    {
                        // Another user is associated with this receipt
                        AppRecommendation appRecommendation = DataReader.GetAppRecommendation(app.Id, me.FacebookId);
                        if (appRecommendation != null)
                        {
                            // Another user already posted a recommendation for this app using this receipt
                            // TODO show error page
                        }

                        // Well that user logged in, but didn't submit a recommendation
                        // That's ok, continue
                    }
                }

                // App + Person is cool

                // AppReceipt should already be in the database
                // Update it with the Facebook Id of the user
                appReceipt.PersonFacebookId = me.FacebookId;
                DataWriter.AddAppReceipt(appReceipt);

                IList<Person> friends = await facebookOperations.GetFriendsAsync();
                CurrentUser currentUser = new CurrentUser
                {
                    Person = me,
                    Friends = friends
                };

                DataWriter.AddFacebookOperations(re_auth_token, facebookOperations);
                DataWriter.AddPersonAndFriends(currentUser);

                string viewName = String.Format("{0}/recommend", platform);
                var viewModel = new RecommendViewModel(currentUser, app, re_auth_token);
                return View(viewName, viewModel);
            }

            throw new InvalidOperationException();
        }

        [HttpPost]
        [RequireHttps]
        public async Task<ActionResult> PostRecommendation(string platform, string re_auth_token, string message)
        {
            AppAuthorization appAuthorization = GetAppAuthorization(re_auth_token, true);
            if (appAuthorization != null)
            {
                var facebookOperations = DataReader.GetFacebookOperations(re_auth_token);
                if (facebookOperations != null)
                {
                    var currentUser = await facebookOperations.GetCurrentUserAsync();
                    var recommendation = new AppRecommendation
                        {
                            AppId = appAuthorization.App.Id,
                            PersonFacebookId = currentUser.FacebookId,
                            UserMessage = message
                        };

                    var postedRecommendation = await facebookOperations.PostAppRecommendationAsync(recommendation);
                    DataWriter.AddAppRecommendation(postedRecommendation);
                    return Json(new
                        {
                            success = true
                        });
                }
            }

            return Json(new
            {
                success = false
            });
        }

        private AppAuthorization GetAppAuthorization(string referEngineAuthToken, bool shouldBeVerified)
        {
            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            AppAuthorization appAuthorization = DataReader.GetAppAuthorization(referEngineAuthToken, userHostAddress);
            const int maxTryCount = 10;
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
