using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using ReferEngine.Common.Models;
using ReferEngine.Web.DataAccess;
using ReferLib;

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

                    CryptoConfig.AddAlgorithm(typeof (RSAPKCS1SHA256SignatureDescription),
                                              "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(appReceiptXmlStr);
                    XmlNode node = xmlDoc.DocumentElement;
                    string certificateId = node.Attributes["CertificateId"].Value;
                    X509Certificate2 verificationCertificate = await RetrieveCertificate(certificateId);

                    if (ValidateXml(xmlDoc, verificationCertificate))
                    {
                        XmlNode appReceiptNode = node.SelectSingleNode("descendant::AppReceipt");
                        if (appReceiptNode != null)
                        {
                            string receiptId = appReceiptNode.Attributes.GetNamedItem("Id").Value;
                            string packageFamilyName = appReceiptNode.Attributes.GetNamedItem("AppId").Value;
                            string licenseTypeStr = appReceiptNode.Attributes.GetNamedItem("LicenseType").Value;
                            LicenseType licenseType = AppReceipt.GetLicenseType(licenseTypeStr);
                            DateTime purchaseDate =
                                Convert.ToDateTime(appReceiptNode.Attributes.GetNamedItem("PurchaseDate").Value);

                            App app = DatabaseOperations.GetApp(packageFamilyName);

                            AppReceipt appReceipt = new AppReceipt(receiptId, app.Id,
                                                                   packageFamilyName, purchaseDate, licenseType);
                            DataWriter.AddAppReceipt(appReceipt);

                            string userHostAddress = HttpContext.Current.Request.UserHostAddress;
                            AppAuthorization appAuthorization = new AppAuthorization(app, appReceipt, userHostAddress);
                            DataWriter.AddAppAuthorization(appAuthorization, TokenExpiresIn);

                            return new JsonResult()
                                       {
                                           Data = new
                                                      {
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

            }

            return new JsonResult()
            {
                Data = new { success = false }
            };
        }

        public async Task<JsonResult> Get()
        {
            return new JsonResult()
                {
                    Data = "what!"
                };
        }

        private static int ReadResponseBytes(byte[] responseBuffer, Stream resStream)
        {
            int count = 0;

            int numBytesRead = 0;
            int numBytesToRead = responseBuffer.Length;

            do
            {
                count = resStream.Read(responseBuffer, numBytesRead, numBytesToRead);

                numBytesRead += count;
                numBytesToRead -= count;

            } while (count > 0);

            return numBytesRead;
        }

        private static async Task<X509Certificate2> RetrieveCertificate(string certificateId)
        {
            // http://msdn.microsoft.com/en-us/library/windows/apps/windows.applicationmodel.store.currentapp.getappreceiptasync.aspx
            String certificateUrl = String.Format("https://go.microsoft.com/fwlink/?LinkId=246509&cid={0}", certificateId);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(certificateUrl);
            responseMessage.EnsureSuccessStatusCode();
            byte[] responseBuffer = await responseMessage.Content.ReadAsByteArrayAsync();

            return new X509Certificate2(responseBuffer);
        }

        private static bool ValidateXml(XmlDocument receipt, X509Certificate2 certificate)
        {
            SignedXml signedXml = new SignedXml(receipt);
            XmlNode signatureNode = receipt.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl)[0];
            if (signatureNode == null)
            {
                return false;
            }

            signedXml.LoadXml((XmlElement)signatureNode);
            return signedXml.CheckSignature(certificate, true);
        }
    }
}
