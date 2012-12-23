using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Web.DataAccess;

namespace ReferEngine.Web.Controllers.Win8
{
    public class AuthController : ApiController
    {
        private IReferDataReader DataReader { get; set; }
        private IReferDataWriter DataWriter { get; set; }
        private static readonly TimeSpan TokenExpiresIn = TimeSpan.FromMinutes(20);

        public AuthController(IReferDataReader dataReader, IReferDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }

        // POST api/win8/auth
        public async Task<JsonResult> Post([FromBody]string value)
        {
            try
            {
                string requestContent = await Request.Content.ReadAsStringAsync();
                if (requestContent != null && requestContent.Length > 1)
                {
                    string streamStr = requestContent.Replace("\0", "");

                    int start = streamStr.IndexOf("<Receipt", 0, StringComparison.OrdinalIgnoreCase);
                    int end = streamStr.IndexOf("</Receipt>", 0, StringComparison.OrdinalIgnoreCase) + 10;
                    string appReceiptXmlStr = streamStr.Substring(start, end - start);

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

                            AppReceipt appReceipt = new AppReceipt
                            {
                                Id = receiptId,
                                AppPackageFamilyName = packageFamilyName,
                                LicenseType = licenseType,
                                PurchaseDate = purchaseDate,
                                AppId = app.Id,
                                XmlContent = appReceiptXmlStr,
                                CertificateId = certificateId
                            };

                             string userHostAddress = HttpContext.Current.Request.UserHostAddress;
                             AppAuthorization appAuthorization = new AppAuthorization(app, appReceipt, userHostAddress);
                             DataWriter.AddAppAuthorization(appAuthorization);

                             return new JsonResult
                             {
                                 Data = new {
                                                success = true,
                                                token = appAuthorization.Token,
                                                expiresIn = TokenExpiresIn.TotalSeconds
                                            }
                             };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //TODO Exception handling
                Trace.WriteLine(e.Message);
            }

            return new JsonResult { Data = new { success = false } };
        }
    }
}
