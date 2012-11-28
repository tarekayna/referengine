using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Facebook;
using ReferLib;

namespace ReferEngine.Controllers
{
    public class FriendsViewModel
    {
        public dynamic Me { get; private set; }
        public dynamic Friends { get; private set; }

        public FriendsViewModel(dynamic me, dynamic friends)
        {
            Me = me;
            Friends = friends;
        }
    }

    public class ReferController : Controller
    {
        private readonly ReferDb _db = new ReferDb();

        public ActionResult Friends()
        {
            string userAccessToken = Request["access_token"];
            if (userAccessToken != null)
            {
                var client = new FacebookClient(userAccessToken);
                dynamic me = client.Get("me");
                dynamic friends = client.Get("me/friends?fields=picture,name,first_name");
                FriendsViewModel viewModel = new FriendsViewModel(me, friends);
                client.AppId = "368842109866922";
                client.AppSecret = "b673f45aa978225ae8c9e4817a726be7";

                //dynamic appAccess = client.Get(
                //   "https://graph.facebook.com/oauth/access_token?client_id=368842109866922&client_secret=b673f45aa978225ae8c9e4817a726be7&grant_type=client_credentials");

                dynamic parameters = new ExpandoObject();
                //parameters.app = "http://apps.facebook.com/referengine/2";
                parameters.app = "http://127.0.0.1/referengine/2";
                //parameters.access_token = appAccess.access_token;
                parameters.access_token = userAccessToken;
                client.Post("me/referengine:refer", parameters);
                return View(viewModel);
            }

            return RedirectToRoute("Start");
        }

        public ActionResult Start(string id)
        {
            int inputId = Convert.ToInt32(id);
            App app = _db.Apps.First(a => a.Id == inputId);
            return View(app);
        }

        public ActionResult Intro(string id)
        {
            int inputId = Convert.ToInt32(id);
            App app = _db.Apps.First(a => a.Id == inputId);
            return View(app);
        }
    }
}
