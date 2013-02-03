using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using ReferEngine.Common.Models;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ReferEngine.Common.Utilities
{
    public static class Util
    {
        public enum ReferEngineServiceConfiguration
        {
            ProductionCloud = 0,
            TestCloud,
            Local
        }

        public static ReferEngineServiceConfiguration CurrentServiceConfiguration
        {
            get
            {
                string currentServiceConfiguration = RoleEnvironment.GetConfigurationSettingValue("CurrentServiceConfiguration");
                switch (currentServiceConfiguration)
                {
                    case "ProductionCloud":
                        return ReferEngineServiceConfiguration.ProductionCloud;

                    case "TestCloud":
                        return ReferEngineServiceConfiguration.TestCloud;

                    case "Local":
                        return ReferEngineServiceConfiguration.Local;
                }

                throw new InvalidDataException(string.Format("Invalid currentServiceConfiguration: {0}",
                                                             currentServiceConfiguration));
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

        public static string DatabaseConnectionString
        {
            get
            {
                const string connectionFormat =
                    "Server=tcp:fnx5xvuqzn.database.windows.net,1433;Database={0};User ID=tarek990@fnx5xvuqzn;Password=r6g4d2hA..;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;MultipleActiveResultSets=True";
                switch (CurrentServiceConfiguration)
                {
                    case ReferEngineServiceConfiguration.ProductionCloud:
                        return string.Format(connectionFormat, "referengine_db");
                    case ReferEngineServiceConfiguration.TestCloud:
                        return string.Format(connectionFormat, "referengine_db_test");
                    case ReferEngineServiceConfiguration.Local:
                        return string.Format(connectionFormat, "referengine_db_local");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static string DatabaseConnectionStringName
        {
            get
            {
                string format = CurrentServiceConfigurationString + "{0}";
                return string.Format(format, "ConnectionString");
            }
        }

        public static string MixPanelProjectToken
        {
            get
            {
                switch (CurrentServiceConfiguration)
                {
                    case ReferEngineServiceConfiguration.ProductionCloud:
                        return "d76136086d701abbecf55a6de775127c";
                    case ReferEngineServiceConfiguration.TestCloud:
                        return "8b975bba223e30932a1ef8cd028f1c1c";
                    case ReferEngineServiceConfiguration.Local:
                        return "6a8d387692d3e39d9b7851edbfcc9b8d";
                    default:
                        throw new ArgumentOutOfRangeException("CurrentServiceConfiguration");
                }
            }
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

        public static bool VerifyAppAuthorization(AppAuthorization appAuthorization)
        {
            try
            {
                CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),
                                          "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

                X509Certificate2 verificationCertificate = RetrieveCertificate(appAuthorization.AppReceipt.CertificateId);
                if (verificationCertificate != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(appAuthorization.AppReceipt.XmlContent);
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
            String certificateUrl = String.Format("https://go.microsoft.com/fwlink/?LinkId=246509&cid={0}", certificateId);

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

            signedXml.LoadXml((XmlElement)signatureNode);
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
    }
}