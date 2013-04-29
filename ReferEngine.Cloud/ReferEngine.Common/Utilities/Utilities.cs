using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Xml;
using ReferEngine.Common.Models;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ReferEngine.Common.Utilities
{
    public static class Util
    {
        private const bool SimulateTestCloud = false;
        private const bool SimulateProductionCloud = false;

        public enum ReferEngineServiceConfiguration
        {
            ProductionCloud = 0,
            TestCloud,
            Local
        }

        public static string BaseUrl
        {
            get
            {
                switch (CurrentServiceConfiguration)
                {
                    case ReferEngineServiceConfiguration.ProductionCloud:
                        return "https://www.referengine.com/";
                    case ReferEngineServiceConfiguration.TestCloud:
                        return "https://www.referengine-test.com/";
                    case ReferEngineServiceConfiguration.Local:
                        return "http://127.0.0.1:81/";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static string CurrentServiceConfigurationString
        {
            get
            {
                switch (CurrentServiceConfiguration)
                {
                    case ReferEngineServiceConfiguration.ProductionCloud:
                        return "ProductionCloud";
                    case ReferEngineServiceConfiguration.TestCloud:
                        return "TestCloud";
                    case ReferEngineServiceConfiguration.Local:
                        return "Local";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static ReferEngineServiceConfiguration CurrentServiceConfiguration
        {
            get
            {
                try
                {
                    if (RoleEnvironment.IsAvailable)
                    {

                        string currentServiceConfiguration =
                            RoleEnvironment.GetConfigurationSettingValue("CurrentServiceConfiguration");
                        switch (currentServiceConfiguration)
                        {
                            case "ProductionCloud":
                                return ReferEngineServiceConfiguration.ProductionCloud;

                            case "TestCloud":
                                return ReferEngineServiceConfiguration.TestCloud;

                            case "Local":
                                {
                                    // ReSharper disable ConditionIsAlwaysTrueOrFalse
                                    // ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
                                    if (SimulateTestCloud) return ReferEngineServiceConfiguration.TestCloud;
                                    if (SimulateProductionCloud) return ReferEngineServiceConfiguration.ProductionCloud;
#pragma warning restore 162
                                    // ReSharper restore HeuristicUnreachableCode
                                    // ReSharper restore ConditionIsAlwaysTrueOrFalse
                                    return ReferEngineServiceConfiguration.Local;
                                }
                        }

                        throw new InvalidDataException(string.Format("Invalid currentServiceConfiguration: {0}",
                                                                     currentServiceConfiguration));
                    }
                    return ReferEngineServiceConfiguration.Local;
                }
                catch (TypeInitializationException)
                {
                    // Unit Testing
                    return ReferEngineServiceConfiguration.ProductionCloud;
                }
            }
        }

        public static string ElmahConnectionString
        {
            get
            {
                const string format = "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";
                switch (CurrentServiceConfiguration)
                {
                    case ReferEngineServiceConfiguration.ProductionCloud:
                        return string.Format(format, "referengineelmah",
                                             "1pMOMS7/z4OfTpO3/DJd6MCfH8NkxQac02Sdis+RPYYNO1puLTQvOA8LIMZDIrygVzqI40RAjw72cdOw4574pA==");
                    case ReferEngineServiceConfiguration.TestCloud:
                        return string.Format(format, "referengineelmahtest",
                                             "5eZciKKweFBSB1POEoQJLKfbGXOhXnYG9tn0FQrst2A3aeQATvAM1LOSlAYwzuSyPyXL1dSyLIRzfSAA70oZdQ==");
                    case ReferEngineServiceConfiguration.Local:
                        return string.Format(format, "referengineelmahlocal",
                                             "oDE32DOEW6k3ckzjdY8Hge0bXvKDVoekRkvIaWzAYugib4ZHONF7y2CZtBO8udhHuhghVj1H9IA8zEkSPQs/TA==");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static string GoogleApiKey
        {
            get { return "AIzaSyAav8gLq0y-uDuWDWZS7DfkVKUkesWMfeg"; }
        }

        public static bool TryConvertToInt(string str, out int result)
        {
            int actualResult = 0;
            try
            {
                actualResult = Convert.ToInt32(str);
                result = actualResult;
                return true;
            }
            catch (FormatException)
            {
                result = actualResult;
                return false;
            }
        }

        public static bool TryConvertToInt64(string str, out Int64 result)
        {
            Int64 actualResult = 0;
            try
            {
                actualResult = Convert.ToInt64(str);
                result = actualResult;
                return true;
            }
            catch (FormatException)
            {
                result = actualResult;
                return false;
            }
        }

        public static bool VerifyAppReceipt(AppReceipt receipt)
        {
            try
            {
                CryptoConfig.AddAlgorithm(typeof (RSAPKCS1SHA256SignatureDescription),
                                          "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

                X509Certificate2 verificationCertificate = RetrieveCertificate(receipt.CertificateId);
                if (verificationCertificate != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(receipt.XmlContent);
                    return ValidateXml(xmlDoc, verificationCertificate);
                }
            }
            catch (Exception e)
            {
                Trace.Write(e.Message);
            }

            return false;
        }

        private static X509Certificate2 RetrieveCertificate(string certificateId)
        {
            // http://msdn.microsoft.com/en-us/library/windows/apps/windows.applicationmodel.store.currentapp.getappreceiptasync.aspx
            String certificateUrl = String.Format("https://go.microsoft.com/fwlink/?LinkId=246509&cid={0}",
                                                  certificateId);

            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(certificateUrl);
            WebResponse webResponse = httpWebRequest.GetResponse();
            Stream receiveStream = webResponse.GetResponseStream();
            if (receiveStream != null)
            {
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string readString = readStream.ReadToEnd();
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] bytes = encoding.GetBytes(readString);

                return new X509Certificate2(bytes);
            }

            return null;
        }

        private static bool ValidateXml(XmlDocument receipt, X509Certificate2 certificate)
        {
            SignedXml signedXml = new SignedXml(receipt);
            XmlNode signatureNode = receipt.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl)[0];
            if (signatureNode == null)
            {
                return false;
            }

            signedXml.LoadXml((XmlElement) signatureNode);
            return signedXml.CheckSignature(certificate, true);
        }

        public static string GetImageBase64String(string path)
        {
            if (path.StartsWith("http"))
            {
                HttpWebRequest httpWebRequest = WebRequest.CreateHttp(path);
                WebResponse webResponse = httpWebRequest.GetResponse();
                Stream receiveStream = webResponse.GetResponseStream();
                if (receiveStream != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    receiveStream.CopyTo(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    return Convert.ToBase64String(bytes);
                }
            }
            else
            {
                byte[] bytes = File.ReadAllBytes(path);
                return Convert.ToBase64String(bytes);
            }
            return null;
        }

        public static string GetMd5Hash(string source)
        {
            StringBuilder hashBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                for (int i = 0; i < data.Length; i++)
                {
                    hashBuilder.Append(data[i].ToString("x2"));
                }
            }
            return hashBuilder.ToString();
        }

        private static string[][] replaceSets = 
            {
                new[] {"-", "_l_"},
                new[] {" ", "_-_"},
                new[] {"&", "_and_"},
                new[] {"!", "_a_"},
                new[] {"?", "_b_"},
                new[] {".", "_c_"},
                new[] {"'", "_d_"},
                new[] {"/", "_e_"},
                new[] {"\\", "_g_"},
                new[] {":", "_h_"},
                new[] {"<", "_i_"},
                new[] {">", "_j_"},
                new[] {"+", "_p_"},
                new[] {"*", "_s_"}
            };
        
        public static string ConvertStringToUrlPart(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            string result = str;
            result = HttpUtility.HtmlDecode(str);
            result = result.ToLower();
            foreach (var replaceSet in replaceSets)
            {
                result = result.Replace(replaceSet[0], replaceSet[1]);
            }
            result = HttpUtility.UrlEncode(result);
            return result;
        }

        public static string ConvertUrlPartToString(string urlPart)
        {
            if (string.IsNullOrEmpty(urlPart)) return null;
            string result = urlPart;
            result = HttpUtility.UrlDecode(result);
            foreach (var replaceSet in replaceSets)
            {
                result = result.Replace(replaceSet[1], replaceSet[0]);
            }
            if (string.IsNullOrEmpty(result)) return null;
            return result;
        }
    }
}