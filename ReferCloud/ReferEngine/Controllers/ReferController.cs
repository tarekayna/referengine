using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Facebook;

namespace ReferEngine.Controllers
{
    public class Device
    {
        public string Name { get; set; }
        public string OS { get; set; }

        public Device(string name, string os)
        {
            this.Name = name;
            this.OS = os;
        }
    }

    public class Person
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public IList<Device> Devices { get; set; }

        public Person(long id, string name)
        {
            this.ID = id;
            this.Name = name;
            Devices = new List<Device>();
        }
    }

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

                dynamic appAccess = client.Get(
                   "https://graph.facebook.com/oauth/access_token?client_id=368842109866922&client_secret=b673f45aa978225ae8c9e4817a726be7&grant_type=client_credentials");

                dynamic parameters = new ExpandoObject();
                parameters.app = "http://www.referengine.com/app/details/2";
                //parameters.access_token = appAccess.access_token;
                parameters.access_token = userAccessToken;
                client.Post("me/referengine:refer", parameters);
                return View(viewModel);
            }

            return RedirectToRoute("Start");
        }

        public ActionResult Start()
        {
            return View();
        }

        public ActionResult Intro()
        {
            return View();
        }


    }
}
