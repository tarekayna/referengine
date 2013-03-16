using System;
using System.Diagnostics;
using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.Facebook;

namespace ReferEngine.Web.Controllers
{
    public class FacebookController : BaseController
    {
        public FacebookController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public ActionResult App(long id, string fb_action_ids, string fb_source, string action_object_map,
            string action_type_map, string action_ref_map)
        {
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);
            App app = DataReader.GetApp(id);

            FacebookPageViewInfo viewInfo = new FacebookPageViewInfo()
                                                    {
                                                        TimeStamp = DateTime.UtcNow,
                                                        ActionId = fb_action_ids,
                                                        AppId = id,
                                                        IpAddress = Request.UserHostAddress
                                                    };
            ServiceBusOperations.AddToQueue(viewInfo);

            return View(new FacebookAppViewModel(app, userAgentProperties));
        }

        // TODO: For now, the facebook home of refer engine will redirect to the app page
        public ActionResult Home()
        {
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);
            App app = DataReader.GetApp(21);

            return View("App", new FacebookAppViewModel(app, userAgentProperties));
        }
    }
}
