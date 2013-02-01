using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class PricingController : BaseController
    {
        public PricingController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public ActionResult Index()
        {
            return View();
        }
    }
}
