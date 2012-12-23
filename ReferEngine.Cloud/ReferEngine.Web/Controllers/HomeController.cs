using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Web.DataAccess;

namespace ReferEngine.Web.Controllers
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
        private IReferDataReader DataReader { get; set; }
        private IReferDataWriter DataWriter { get; set; }

        public HomeController(IReferDataReader dataReader, IReferDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }

        [HttpPost]
        public ActionResult Index(string email)
        {
            if (email == null)
            {
                throw new InvalidOperationException();
            }

            PrivateBetaSignup privateBetaSignup = new PrivateBetaSignup(email);
            DataWriter.AddPrivateBetaSignup(privateBetaSignup);
            return Json(new { Email = email });           
        }

        [HttpGet]
        public ActionResult Index()
        {
            LandingPageVariation landingPageToUse = GetLandingPage();
            ViewBag.LandingPageName = landingPageToUse.Name;
            SetViewProperties(landingPageToUse.ViewProperties);

            return View();
        }

        private void SetViewProperties(ViewProperties viewProperties)
        {
            ViewBag.ButtonClass = viewProperties.ButtonClass;
            ViewBag.HeadlineText = viewProperties.HeadlineText;
            ViewBag.FocusOnInput = viewProperties.FocusOnInput;
        }

        private static LandingPageVariation GetLandingPage()
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
                                                                                       HeadlineText = "Your app can go viral with Refer Engine. Increase your app sales and reward your customers."
                                                                                   }
                                                          },
                                                      new LandingPageVariation 
                                                          {
                                                              Name = "GoViralOrange",
                                                              Probability = 25,
                                                              ViewProperties = new ViewProperties
                                                                                   {
                                                                                       HeadlineText = "Your app can go viral with Refer Engine. Increase your app sales and reward your customers.",
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
    }
}
