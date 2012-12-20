using ReferEngine.Common.Models;
using ReferEngine.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ReferEngine.Web.Models.Refer.Win8;

namespace ReferEngine.Web.Controllers
{
    public class ReferController : Controller
    {
        private IReferDataReader DataReader { get; set; }
        private IReferDataWriter DataWriter { get; set; }

        public ReferController(IReferDataReader dataReader, IReferDataWriter dataWriter)
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
            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            AppAuthorization appAuthorization = DataReader.GetAppAuthorization(re_auth_token, userHostAddress);
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

            //TODO
            throw new Exception();
        }

        [HttpPost]
        public async Task<ActionResult> PostRecommendation(string platform, string re_auth_token, string message)
        {
            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            AppAuthorization appAuthorization = DataReader.GetAppAuthorization(re_auth_token, userHostAddress);
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
    }
}
