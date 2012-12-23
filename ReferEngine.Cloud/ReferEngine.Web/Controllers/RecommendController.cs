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
        public ActionResult Intro(string platform, long id)
        {
            App app = DataReader.GetApp(id);
            string viewName = String.Format("{0}/intro", platform);
            return View(viewName, app);
        }

        [OutputCache(Duration = 0)]
        public async Task<ActionResult> Recommend(string platform, string re_auth_token, string fb_access_code)
        {
            AppAuthorization appAuthorization = GetAppAuthorization(re_auth_token, false);
            if (appAuthorization != null)
            {
                App app = appAuthorization.App;
                string accessCode = fb_access_code.Replace("%23", "#");
                FacebookOperations facebookOperations = await FacebookOperations.CreateAsync(accessCode);

                Person me = await facebookOperations.GetCurrentUserAsync();
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
