using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.Account;
using ReferEngine.Web.Models.Common;
using WebMatrix.WebData;

namespace ReferEngine.Web.Controllers
{
    //[InitializeSimpleMembership]
    [RequireHttpsPermanentRemote]
    public class BaseController : Controller
    {
        protected IReferDataReader DataReader { get; set; }
        protected IReferDataWriter DataWriter { get; set; }
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public BaseController(IReferDataReader dataReader, IReferDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            ViewData["ViewProperties"] = new ViewProperties();
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);

            if (User.Identity.IsAuthenticated)
            {
                viewProperties.CurrentUser = DataReader.GetUser(WebSecurity.CurrentUserId);
            }

            base.OnActionExecuting(filterContext);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection(Util.DatabaseConnectionStringName, "Users", "Id", "Email", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
