using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Facebook;

namespace ReferEngine.Controllers
{
    internal class ViewProperties
    {
        public string ButtonClass { get; set; }
        public string HeadlineText { get; set; }
        public bool FocusOnInput { get; set; }

        internal ViewProperties()
        {
            FocusOnInput = false;
            HeadlineText = "Increase your app sales and reward your customers. It's a win-win!";
            ButtonClass = "btn-primary";
        }
    }

    internal class LandingPageVariation
    {
        public string Name { get; set; }
        public int Probability { get; set; }
        public ViewProperties ViewProperties { get; set; }
    }

    public class HomeController : Controller
    {
        internal void SetViewProperties(ViewProperties viewProperties)
        {
            ViewBag.ButtonClass = viewProperties.ButtonClass;
            ViewBag.HeadlineText = viewProperties.HeadlineText;
            ViewBag.FocusOnInput = viewProperties.FocusOnInput;
        }

        internal LandingPageVariation GetLandingPage()
        {
            List<LandingPageVariation> list = new List<LandingPageVariation>
                                                  {
                                                      new LandingPageVariation 
                                                          {
                                                              Name = "Base",
                                                              Probability = 20,
                                                              ViewProperties = new ViewProperties()
                                                          },
                                                      new LandingPageVariation 
                                                          {
                                                              Name = "WinWinFirst",
                                                              Probability = 20,
                                                              ViewProperties = new ViewProperties
                                                                                   {
                                                                                       HeadlineText = "It's a win-win! Increase your app sales and reward your customers."
                                                                                   }
                                                          },
                                                      new LandingPageVariation 
                                                          {
                                                              Name = "GoViral",
                                                              Probability = 25,
                                                              ViewProperties = new ViewProperties
                                                                                   {
                                                                                       HeadlineText = "Your app 2 can go viral with Refer Engine. Increase your app sales and reward your customers."
                                                                                   }
                                                          },
                                                      new LandingPageVariation 
                                                          {
                                                              Name = "GoViralOrange",
                                                              Probability = 25,
                                                              ViewProperties = new ViewProperties
                                                                                   {
                                                                                       HeadlineText = "Your app 2 can go viral with Refer Engine. Increase your app sales and reward your customers.",
                                                                                       ButtonClass = "btn-warning"
                                                                                   }
                                                          },
                                                      new LandingPageVariation
                                                          {
                                                              Name = "OrangeButton",
                                                              Probability = 10,
                                                              ViewProperties = new ViewProperties
                                                                                   {
                                                                                       ButtonClass = "btn-warning"
                                                                                   }
                                                          },
                                                      new LandingPageVariation
                                                          {
                                                              Name = "RedButton",
                                                              Probability = 0,
                                                              ViewProperties = new ViewProperties
                                                                                   {
                                                                                       ButtonClass = "btn-danger"
                                                                                   }
                                                          },
                                                      new LandingPageVariation
                                                          {
                                                              Name = "GreenButton",
                                                              Probability = 0,
                                                              ViewProperties = new ViewProperties
                                                                                   {
                                                                                       ButtonClass = "btn-success"
                                                                                   }
                                                          }
                                                  };

            List<LandingPageVariation> weightedList = new List<LandingPageVariation>();
            foreach (LandingPageVariation landingPage in list)
            {
                for (int j = 0; j < landingPage.Probability; j++)
                {
                    weightedList.Add(landingPage);   
                }
            }
            Random random = new Random((int)DateTime.Now.Ticks);
            
            LandingPageVariation landingPageToUse = false ? list[0] : weightedList[random.Next(0, weightedList.Count - 1)];

            return landingPageToUse;
        }

        internal ActionResult ProcessPostbackAndSendEmails()
        {
            string email = Request.Form["Email"];

            DataAccess.DataAccess.InsertPrivateBetaEmail(email);

            // Send the customer a thank you email
            string body = "Hello! " + "\n\n";
            body += "Thank you for signing up for Refer Engine's private beta. We will let you as soon as we are ready.\n\n";
            body += "Tarek\n";
            body += "ReferEngine.com Founder and Developer";

            try
            {
                MailAddress from = new MailAddress("tarek@referengine.co", "Tarek, ReferEngine.com");
                const string subject = "ReferEngine.com Signup";

                var to = new MailAddress(email);
                var mailMessage = new MailMessage(from, to)
                {
                    Body = body,
                    IsBodyHtml = false,
                    Sender = from,
                    Subject = subject
                };
                mailMessage.ReplyToList.Add(to);
                var client = new SmtpClient("smtp.mandrillapp.com") { Port = 587 };
                const string username = "tarek@apexa.co";
                const string password = "38b99f5a-45aa-4dab-8c94-452718b2afee";
                client.Credentials = new NetworkCredential(username, password);
                client.Send(mailMessage);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }

            return Json(new { Email = email });
        }

        public ActionResult Index()
        {
            if (Request.HttpMethod == "POST")
            {
                return ProcessPostbackAndSendEmails();
            }

            LandingPageVariation landingPageToUse = GetLandingPage();
            ViewBag.LandingPageName = landingPageToUse.Name;
            SetViewProperties(landingPageToUse.ViewProperties);

            return View("Index");
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
