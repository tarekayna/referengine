using System.Web.Mvc;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Filters;
using ReferEngine.Web.Models.Common;
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
            ViewProperties.PageTitle = string.Empty;

            if (User.Identity.IsAuthenticated)
            {
                ViewProperties.CurrentUser = DataReader.GetUser(WebSecurity.CurrentUserId);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
