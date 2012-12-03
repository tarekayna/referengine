using System;
using System.Collections.Generic;
using Facebook;
using ReferEngine.DataAccess;
using ReferEngine.Models.Error;
using ReferEngine.Models.Refer.Win8;
using ReferEngine.Utilities;
using ReferLib;
using System.Dynamic;
using System.Web.Mvc;

namespace ReferEngine.Controllers
{
    public class ReferController : Controller
    {
        [HttpGet]
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
                        const string meRequest =
                            "me?fields=id,name,devices,first_name,last_name,email,gender,address,birthday,picture,relationship_status,timezone,verified,work";
                        dynamic me = facebookClient.Get(meRequest);
                        Person user = new Person(me);
                        //DataOperations.AddPerson(user);

                        //dynamic parameters = new ExpandoObject();
                        //parameters.app = "http://apps.facebook.com/referengine/app/" + id;
                        //parameters.access_token = userAccessToken;
                        //dynamic postResult = facebookClient.Post("me/referengine:referto", parameters);
                        //Int64 postId;
                        //if (Util.TryConvertToInt64(postResult.id, out postId))
                        //{
                        //    AppReferral referral = new AppReferral(app.Id, postId, user.FacebookId);
                        //    DataOperations.AddReferral(referral);
                        //}

                        // Get friends list and pass on to the view
                        dynamic friends = facebookClient.Get("me/friends?fields=picture,name,first_name");

                        return View(platform + "/friends", new FriendsViewModel(user, friends, app, userAccessToken));
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
        
        // refer/win8/postToTimeline/id?userAccessToken=#&friendId=#
        [HttpPost]
        public ActionResult PostToTimeline(string platform, string id, string userAccessToken, string friendId)
        {
            if (userAccessToken != null)
            {
                int inputId;
                if (id != null && Util.TryConvertToInt(id, out inputId))
                {
                    App app;
                    if (DataOperations.TryGetApp(inputId, out app))
                    {
                        FacebookClient client = new FacebookClient(userAccessToken);
                        Dictionary<string,object> parameters = new Dictionary<string, object>();
                        parameters["message"] = "Hey";
                        parameters["picture"] = app.ImageLink;
                        parameters["link"] = "http://apps.facebook.com/referengine/app" + app.Id;
                        parameters["name"] = app.Name;
                        parameters["description"] = app.Description;
                        parameters["access_token"] = userAccessToken;
                        var post = client.Post(friendId + "/feed", parameters);
                        return Json(new { result = "success" });
                    }
                }
            }

            return Json(new { result = "error" });
        }

        [OutputCache(Duration=0)]
        public ActionResult Intro(string platform, string id)
        {
            int inputId;
            if (id != null && Util.TryConvertToInt(id, out inputId))
            {
                App app;
                if (DataOperations.TryGetApp(inputId, out app))
                {
                    return View(platform + "/intro", app);
                }
            }

            return ErrorResult("Invalid App ID.");
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
