using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.Models.Account;
using ReferEngine.Web.Models.Common;
using WebMatrix.WebData;

namespace ReferEngine.Web.Controllers
{
    //[InitializeSimpleMembership]
    [RequireHttpsPermanentRemote]
    public class BaseController : Controller
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        protected static class SessionKey
        {
            public const string FacebookAccessSession = "FacebookAccessSession";
            public const string CurrentUser = "CurrentUser";
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            ViewData["ViewProperties"] = new ViewProperties();
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);

            if (User.Identity.IsAuthenticated)
            {
                if (Session[SessionKey.CurrentUser] == null)
                {
                    User user = DataOperations.GetUser(User.Identity.Name);
                    Session[SessionKey.CurrentUser] = user;
                    viewProperties.CurrentUser = user;
                }
                else
                {
                    viewProperties.CurrentUser = (User)(Session[SessionKey.CurrentUser]);                    
                }
                viewProperties.FacebookAccessSession = (FacebookAccessSession)(Session[SessionKey.FacebookAccessSession]);
            }

            base.OnActionExecuting(filterContext);
        }

// ReSharper disable ClassNeverInstantiated.Local
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

                    WebSecurity.InitializeDatabaseConnection(DbConnector.GetCurretConnectionStringName(), "Users", "Id", "Email", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
// ReSharper restore ClassNeverInstantiated.Local
}
