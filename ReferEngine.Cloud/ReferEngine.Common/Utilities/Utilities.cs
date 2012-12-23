using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Utilities
{
    public static class Util
    {
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
    }
}