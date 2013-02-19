using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.Common;
using ReferEngine.Web.Models.Recommend.Win8;
using StackExchange.Profiling;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;


namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    [OutputCache(Duration = 0)]
    public class RecommendController : BaseController
    {
        public RecommendController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public ActionResult Intro(string platform, long id, string authToken, bool isAutoOpen = false)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(id, "id");
            Verifier.IsNotNullOrEmpty(authToken, "authToken");

            var appAuthorization = GetAppAuthorization(authToken);

            DatabaseOperations.AddRecommendationPageView(appAuthorization, RecommendationPage.Intro, isAutoOpen);

            ViewProperties.CurrentApp = DataReader.GetApp(id);
            string viewName = String.Format("{0}/intro", platform);
            return View(viewName, ViewProperties.CurrentApp);
        }

        public ActionResult RecommendViewOnly(string platform, long id)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(id, "id");

            ViewProperties.CurrentApp = DataReader.GetApp(id);

            // Facebook is cool
            Person me = DatabaseOperations.GetPerson(509572882);

            string viewName = String.Format("{0}/recommend", platform);
            var viewModel = new RecommendViewModel(me, ViewProperties.CurrentApp, "fake_auth_token", null);
            return View(viewName, viewModel);
        }

        public async Task<ActionResult> Recommend(string platform, string authToken, string facebookCode)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(authToken, "authToken");
            Verifier.IsNotNullOrEmpty(facebookCode, "facebookCode");

            var profiler = MiniProfiler.Current;

            using (profiler.Step("Recommend"))
            {
                bool showErrorView = false;
                AppAuthorization appAuthorization = GetAppAuthorization(authToken);

                DatabaseOperations.AddRecommendationPageView(appAuthorization, RecommendationPage.Post);

                // App is cool
                ViewProperties.CurrentApp = appAuthorization.App;
                string accessCode = facebookCode.Replace("%23", "#");
                FacebookOperations facebookOperations = await FacebookOperations.CreateAsync(accessCode, authToken);

                // Facebook is cool
                Person me = await facebookOperations.GetCurrentUserAsync();

                AppReceipt appReceipt = appAuthorization.AppReceipt;
                AppReceipt existingReceipt = DataReader.GetAppReceipt(appReceipt.Id);
                if (existingReceipt != null && existingReceipt.PersonFacebookId.HasValue)
                {
                    // App + Person: something is up
                    if (existingReceipt.PersonFacebookId != me.FacebookId)
                    {
                        // Another user is associated with this receipt
                        AppRecommendation appRecommendation = DataReader.GetAppRecommendation(ViewProperties.CurrentApp.Id,
                                                                                              existingReceipt.PersonFacebookId.Value);
                        if (appRecommendation != null)
                        {
                            // Another user already posted a recommendation for this app using this receipt
                            showErrorView = true;
                        }
                        //else
                        //{
                              // Well that user logged in, but didn't submit a recommendation
                              // That's ok, continue
                        //}
                    }
                    //else
                    //{
                    //    // Current user is already associated with this receipt
                    //    // Two options:
                    //    //          1- Person has already posted a recommendation for this app using this receipt
                    //    //             It's fine, they can post again
                    //    //          2- Person has logged in before but did not submit a recommendation
                    //    //             That's ok, continue
                    //}
                }

                if (showErrorView)
                {
                    string viewName = String.Format("{0}/recommend-error", platform);
                    var viewModel = new RecommendViewModel(me, ViewProperties.CurrentApp, authToken, appReceipt);
                    return View(viewName, viewModel);
                }
                else
                {
                    // AppReceipt should already be in the database
                    // Update it with the Facebook Id of the user
                    appReceipt.PersonFacebookId = me.FacebookId;
                    DataWriter.AddAppReceipt(appReceipt);

                    using (profiler.Step("DataWriter.AddFacebookOperations"))
                    {
                        DataWriter.AddFacebookOperations(authToken, facebookOperations);
                    }

                    string viewName = String.Format("{0}/recommend", platform);
                    var viewModel = new RecommendViewModel(me, ViewProperties.CurrentApp, authToken, appReceipt);
                    return View(viewName, viewModel);
                }
            }
        }

        [HttpPost]
        public JsonResult AuthorizeApp(string platform, string appReceiptXml)
        {
            string userIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            TimeSpan tokenExpiresIn = TimeSpan.FromHours(4);

            Stream stream = System.Web.HttpContext.Current.Request.GetBufferedInputStream();
            StreamReader reader = new StreamReader(stream);
            string requestBody = reader.ReadToEnd();
            int start = requestBody.IndexOf("<Receipt", 0, StringComparison.OrdinalIgnoreCase);
            int end = requestBody.IndexOf("</Receipt>", 0, StringComparison.OrdinalIgnoreCase) + 10;

            if (start != -1 && end != -1)
            {
                string appReceiptXmlStr = requestBody.Substring(start, end - start);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(appReceiptXmlStr);
                XmlNode node = xmlDoc.DocumentElement;
                if (node != null && node.Attributes != null)
                {
                    string certificateId = node.Attributes["CertificateId"].Value;
                    XmlNode appReceiptNode = node.SelectSingleNode("descendant::AppReceipt");
                    if (appReceiptNode != null && appReceiptNode.Attributes != null)
                    {
                        string receiptId = appReceiptNode.Attributes.GetNamedItem("Id").Value;
                        string packageFamilyName = appReceiptNode.Attributes.GetNamedItem("AppId").Value;
                        string licenseTypeStr = appReceiptNode.Attributes.GetNamedItem("LicenseType").Value;
                        LicenseType licenseType = AppReceipt.GetLicenseType(licenseTypeStr);
                        DateTime purchaseDate =
                            Convert.ToDateTime(appReceiptNode.Attributes.GetNamedItem("PurchaseDate").Value);
                        App app = DatabaseOperations.GetApp(packageFamilyName);

                        AppReceipt appReceipt = DatabaseOperations.GetAppReceipt(receiptId);
                        if (appReceipt == null)
                        {
                            appReceipt = new AppReceipt
                                                        {
                                                            Id = receiptId,
                                                            AppPackageFamilyName = packageFamilyName,
                                                            LicenseType = licenseType,
                                                            PurchaseDate = purchaseDate,
                                                            AppId = app.Id,
                                                            XmlContent = appReceiptXmlStr,
                                                            CertificateId = certificateId
                                                        };
                            appReceipt.Verified = Util.VerifyAppReceipt(appReceipt);
                            DatabaseOperations.AddAppReceipt(appReceipt);
                        }
                        if (appReceipt.Verified)
                        {
                            bool locationIsSupported = Util.CurrentServiceConfiguration == Util.ReferEngineServiceConfiguration.Local;
                            if (!locationIsSupported)
                            {
                                IpAddressLocation location = DatabaseOperations.GetIpAddressLocation(userIpAddress);
                                locationIsSupported = location.Country.Equals("United States",
                                                                              StringComparison.OrdinalIgnoreCase);
                            }

                            if (locationIsSupported)
                            {
                                // TODO: Verify app license

                                AppAuthorization appAuthorization = new AppAuthorization(app, appReceipt)
                                {
                                    TimeStamp = DateTime.UtcNow,
                                    ExpiresAt = DateTime.UtcNow.Add(tokenExpiresIn),
                                    UserHostAddress = userIpAddress
                                };

                                DatabaseOperations.AddAppAuthorization(appAuthorization);

                                AppAutoShowOptions autoShowOptions = DatabaseOperations.GetAppAutoShowOptions(app.Id);

                                string style = System.IO.File.ReadAllText(Server.MapPath(@"~\TypeScript\recommend\windows\client\WindowsClientStyle.min.css"));

                                string[] loadingMessages = new string[]
                                                               {
                                                                    "Recommend to your friends",
                                                                    "Win cash",
                                                                    "Claim it or donate it",
                                                                    "Share the love",
                                                                    "Earn a reward"
                                                               };

                                return new JsonResult
                                {
                                    Data = new
                                    {
                                        token = appAuthorization.Token,
                                        expiresIn = tokenExpiresIn.TotalSeconds,
                                        asoEnabled = autoShowOptions.Enabled,
                                        asoInterval = autoShowOptions.Interval,
                                        asoTimeout = autoShowOptions.Timeout,
                                        style,
                                        loadingMessages,
                                        logoMarkImageData = "data:image/png;base64,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",
                                        logoTextImageData = "data:image/png;base64,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",
                                        fbScope = "email,publish_actions"
                                    }
                                };
                            }

                            throw new InvalidOperationException("Location is not supported.");
                        }

                        throw new InvalidOperationException("Could not verify app receipt.");
                    }

                    throw new InvalidOperationException("Invalid AppReceiptNode");
                }

                throw new InvalidOperationException("Invalid XmlNode");
            }

            throw new InvalidOperationException("Invalid Request.Content");
        }

        [HttpPost]
        public async Task<ActionResult> PostRecommendation(string platform, string authToken, string message)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(authToken, "authToken");

            AppAuthorization appAuthorization = GetAppAuthorization(authToken);

            var facebookOperations = DataReader.GetFacebookOperations(authToken);
            Verifier.IsNotNullOrEmpty(facebookOperations, "facebookOperations");
            
            var me = await facebookOperations.GetCurrentUserAsync();
            var recommendation = new AppRecommendation
                                        {
                                            AppId = appAuthorization.App.Id,
                                            PersonFacebookId = me.FacebookId,
                                            UserMessage = message,
                                            IpAddress = System.Web.HttpContext.Current.Request.UserHostAddress
                                        };

            var postedRecommendation = await facebookOperations.PostAppRecommendationAsync(recommendation);
            DataWriter.AddAppRecommendation(postedRecommendation);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<ActionResult> GetFriends(string platform, string authToken)
        {
            Verifier.IsNotNullOrEmpty(platform, "platform");
            Verifier.IsNotNullOrEmpty(authToken, "authToken");

            GetAppAuthorization(authToken);

            var facebookOperations = DataReader.GetFacebookOperations(authToken);
            Verifier.IsNotNullOrEmpty(facebookOperations, "facebookOperations");

            var friends = await facebookOperations.GetFriendsAsync();

            // Add this here so we don't have to request the friends 
            // from the Worker. The token might have expired.
            ServiceBusOperations.AddToQueue(facebookOperations);
                    
            return Json(new {friends = Person.Serialize(friends)});
        }

        private AppAuthorization GetAppAuthorization(string authToken)
        {
            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            var auth = DatabaseOperations.GetAppAuthorization(authToken);
            if (auth.UserHostAddress.Equals(userHostAddress, StringComparison.OrdinalIgnoreCase))
            {
                if (auth.ExpiresAt.CompareTo(DateTime.UtcNow) > 0)
                {
                    return auth;
                }

                throw new InvalidOperationException("AppAuthorization has expired");
            }

            throw new InvalidOperationException("AppAuthorization has a different ip address.");
        }
    }
}
