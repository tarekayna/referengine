using Facebook;
using ReferEngine.DataAccess;
using ReferEngine.Models.Refer.Win8;
using ReferEngine.Utilities;
using ReferLib;
using System.Dynamic;
using System.Web.Mvc;

namespace ReferEngine.Controllers
{
    public class ReferController : Controller
    {
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
                        FacebookClient client = new FacebookClient(userAccessToken);
                        const string meRequest =
                            "me?fields=id,name,devices,first_name,last_name,email,gender,address,birthday,picture,relationship_status,timezone,verified,work";
                        dynamic me = client.Get(meRequest);
                        Person myself = new Person(me);
                        DataOperations.AddPerson(myself);

                        // If not in database, add them and connect them to this app

                        // Post Refer Action
                        dynamic parameters = new ExpandoObject();
                        parameters.app = "http://www.referengine.com/app/details/2";
                        parameters.access_token = userAccessToken;
                        dynamic postResult = client.Post("me/referengine:refer", parameters);

                        // Get friends list and pass on to the view
                        dynamic friends = client.Get("me/friends?fields=picture,name,first_name");
                        
                        return View(platform + "/friends", new FriendsViewModel(myself, friends, app));
                    }
                }
            }
            else
            {
                return View(platform + "/friends", new FriendsViewModel("Missing User Access Token"));
            }

            return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
        }

        public ActionResult Start(string platform, string id)
        {
            int inputId;
            if (id != null && Util.TryConvertToInt(id, out inputId))
            {
                App app;
                if (DataOperations.TryGetApp(inputId, out app))
                {
                    return View(platform + "/start", app);
                }
            }

            return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
        }

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
            
            return RedirectToRoute("Default", new {controller = "Home", action = "Index"});
        }
    }
}
