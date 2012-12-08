using Facebook;
using ReferEngine.DataAccess;
using ReferEngine.Models.Refer.Win8;
using ReferEngine.Utilities;
using ReferLib;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ReferEngine.Controllers
{
    public class ReferController : Controller
    {
        const string RequestFields =
            "?fields=id,name,devices,first_name,last_name,email,gender,address,birthday,picture,relationship_status,timezone,verified,work";

        [OutputCache(Duration=0)]
        public ActionResult Intro(string platform, string id)
        {
            DateTime start = DateTime.Now;
            int inputId;
            if (id != null && Util.TryConvertToInt(id, out inputId))
            {
                App app;
                if (DataOperations.TryGetApp(inputId, out app))
                {
                    ViewBag.TimeSpan = DateTime.Now - start;
                    return View(platform + "/intro", app);
                }
            }

            return ErrorResult("Invalid App ID.");
        }

        [OutputCache(Duration = 0)]
        public ActionResult Friends(string platform, string id, string userAccessToken)
        {
            if (userAccessToken != null)
            {
                int inputId;
                if (id != null && Util.TryConvertToInt(id, out inputId))
                {
                    App app;
                    if (DataOperations.TryGetApp(inputId, out app))
                    {
                        FacebookClient facebookClient = new FacebookClient(userAccessToken);

                        const string meRequest = "me" + RequestFields;
                        dynamic me = facebookClient.Get(meRequest);
                        Person user = new Person(me);
                        //DataOperations.AddPerson(user);

                        //Dictionary<string, object> parameters = new Dictionary<string, object>();
                        //parameters["app"] = "http://apps.facebook.com/referengine/app/" + id;
                        //parameters["access_token"] = userAccessToken;
                        //parameters["fb:explicitly_shared"] = "true";
                        //parameters["message"] = "Great app!. @[tarek.ayna]";
                        //dynamic postResult = facebookClient.Post("me/referengine:recommend", parameters);
                        //Int64 postId;
                        //if (Util.TryConvertToInt64(postResult.id, out postId))
                        //{
                        //    AppReferral referral = new AppReferral(app.Id, postId, user.FacebookId);
                        //    DataOperations.AddReferral(referral);
                        //}

                        // Get friends list and pass on to the view
                        const string friendsRequest = "me/friends" + RequestFields;
                        dynamic friends = facebookClient.Get(friendsRequest);
                        List<Person> friendsList = new List<Person>();
                        for (int i = 0; i < friends.data.Count; i++)
                        {
                            friendsList.Add(new Person(friends.data[i]));
                        }
                        friendsList.Add(user);
                        //DataOperations.AddPersonAndFriends(user, friendsList);

                        return View(platform + "/friends", new FriendsViewModel(user, friendsList, app, userAccessToken));
                    }
                    else
                    {
                        return ErrorResult("Invalid Id.");
                    }
                }
            }
            else
            {
                return ErrorResult("Missing User Access Token");
            }

            return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public ActionResult PostRecommendation(string platform, string id, string userAccessToken, string message)
        {
            if (userAccessToken != null)
            {
                int inputId;
                if (id != null && Util.TryConvertToInt(id, out inputId))
                {
                    App app;
                    if (DataOperations.TryGetApp(inputId, out app))
                    {
                        FacebookClient facebookClient = new FacebookClient(userAccessToken);

                        const string meRequest = "me" + RequestFields;
                        dynamic me = facebookClient.Get(meRequest);
                        Person user = new Person(me);

                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters["app"] = "http://apps.facebook.com/referengine/app/" + id;
                        parameters["access_token"] = userAccessToken;
                        parameters["fb:explicitly_shared"] = "true";
                        parameters["message"] = message;
                        dynamic postResult = facebookClient.Post("me/referengine:recommend", parameters);
                        Int64 postId;
                        if (Util.TryConvertToInt64(postResult.id, out postId))
                        {
                            var recommendation = new AppRecommendation(app.Id, postId, user.FacebookId);
                            DataOperations.AddRecommendation(recommendation);
                        }
                    }
                }
            }

            return new JsonResult();
        }

        private ActionResult ErrorResult(string error)
        {
            return RedirectToAction("Error", "Error", new
            {
                message = error
            });
        }
    }
}
