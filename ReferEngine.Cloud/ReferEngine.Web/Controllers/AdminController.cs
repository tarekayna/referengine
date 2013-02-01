using System.Web.Mvc;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.Admin;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    [Authorize(Roles="Admin")]
    public class AdminController : BaseController
    {
        public AdminController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public ActionResult Admin()
        {
            var viewModel = new AdminViewModel();
            return View(viewModel);
        }

    }
}
