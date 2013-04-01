using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.Models.Facebook;
using System;
using System.Web.Mvc;

namespace ReferEngine.Web.Controllers
{
    public class FacebookController : BaseController
    {
        public ActionResult App(long id, string fb_action_ids, string fb_source, string action_object_map,
            string action_type_map, string action_ref_map)
        {
            App app = DataOperations.GetApp(id);
            return FacebookAppView(app, fb_action_ids);
        }

        public ActionResult AppByName(string platform, string name, string fb_action_ids, string fb_source, string action_object_map,
            string action_type_map, string action_ref_map)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(name, "name");

            string appName = name.Replace('-', ' ');
            App app = DataOperations.GetAppByName(platform, appName);
            return FacebookAppView(app, fb_action_ids);
        }

        private ActionResult FacebookAppView(App app, string fb_action_ids)
        {
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);
            FacebookPageViewInfo viewInfo = new FacebookPageViewInfo
            {
                TimeStamp = DateTime.UtcNow,
                ActionId = fb_action_ids,
                AppId = app.Id,
                IpAddress = Request.UserHostAddress
            };
            ServiceBusOperations.AddToQueue(viewInfo);

            return View("app", new FacebookAppViewModel(app, userAgentProperties));
        }

        // TODO: For now, the facebook home of refer engine will redirect to the app page
        public ActionResult Home()
        {
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);
            App app = DataOperations.GetApp(21);

            return View("App", new FacebookAppViewModel(app, userAgentProperties));
        }
    }
}
