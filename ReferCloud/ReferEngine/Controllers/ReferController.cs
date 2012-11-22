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
            string accessToken = Request["access_token"];
            if (accessToken != null)
            {
                var client = new FacebookClient(accessToken);
                dynamic me = client.Get("me");
                dynamic friends = client.Get("me/friends?fields=picture,name,first_name");
                FriendsViewModel viewModel = new FriendsViewModel(me, friends);
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

        //public ActionResult Facebook()
        //{
        //    if (Request.HttpMethod == "POST")
        //    {
        //        string accessToken = Request.Form["AccessToken"];
        //        //string accessToken = "AAAFPdb7v06oBAOrQh1IUHdNOlDZBuXtG3zZBri1sYY6EmPkXQuF5vyTPgoUKw6z5EdumsE7E7UMIZAjeqbWzEHCEp6lDGecg3clZAzJEnwZDZD";
        //        var client = new FacebookClient(accessToken);
        //        dynamic response = client.Get("me/friends?fields=name,devices");
        //        dynamic data = response.data;

        //        List<Person> peopleWithIPhone = new List<Person>();
        //        List<Person> peopleWithIPad = new List<Person>();
        //        List<Person> peopleWithAndroid = new List<Person>();
        //        List<Person> peopleWithNothing = new List<Person>();
        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            string name = data[i].name;
        //            Int64 id = Convert.ToInt64(data[i].id);
        //            Person person = new Person(id, name);

        //            if (data[i].devices == null)
        //            {
        //                peopleWithNothing.Add(person);
        //            }
        //            else 
        //            {
        //                for (int j = 0; j < data[i].devices.Count; j++)
        //                {
        //                    string deviceName = data[i].devices[j].hardware;
        //                    string os = data[i].devices[j].os;
        //                    if (deviceName != null && deviceName.Equals("iPhone"))
        //                    {
        //                        peopleWithIPhone.Add(person);
        //                    }
        //                    else if (deviceName != null && deviceName.Equals("iPad"))
        //                    {
        //                        peopleWithIPad.Add(person);
        //                    }
        //                    else if (os != null && os.Equals("Android"))
        //                    {
        //                        peopleWithAndroid.Add(person);
        //                    }
        //                }
        //            }

        //            i++;
        //        }

        //        return Json(new 
        //        {
        //            iPhone = peopleWithIPhone,
        //            iPad = peopleWithIPad,
        //            Android = peopleWithAndroid
        //        }
        //        );
        //    }

        //    return View();
        //}
    }
}
