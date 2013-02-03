using System.Web.Mvc;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Filters;
using WebMatrix.WebData;

namespace ReferEngine.Web.Controllers
{
    [InitializeSimpleMembership]
    public class BaseController : Controller
    {
        protected IReferDataReader DataReader { get; set; }
        protected IReferDataWriter DataWriter { get; set; }

        public BaseController(IReferDataReader dataReader, IReferDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = DataReader.GetUser(WebSecurity.CurrentUserId);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
