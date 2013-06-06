using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using AppSmarts.Common.Tracing;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.AzureManagement
{
    public enum Subscription
    {
        [StringValue("28b4b2a5-c946-4fa9-b0ef-37de30a19333")]
        PayAsYouGo
    }

    public enum CloudService
    {
        [StringValue("referengine-ios-queuecruncher")]
        iOSQueueCruncher
    }

    public enum Deployment
    {
        [StringValue("production")]
        Production,

        [StringValue("staging")]
        Staging
    }

    public static class AzureManager
    {
        private static readonly IDictionary<Subscription, X509Certificate> Certificates = new Dictionary<Subscription, X509Certificate>();
        private static X509Certificate GetCertificate(Subscription subscription)
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            certStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint,
                                                                                    "72FD922AC57C5FC56EC4B446F372E67BB8864ABD",
                                                                                    false);

            certStore.Close();
            if (certCollection.Count == 0)
            {
                Tracer.Trace(TraceMessage.Warning("No certificate found"));
                return null;
            }
            return certCollection[0];
        }

        private static Uri GetRequestUri(Subscription subscription, string operation)
        {
           return new Uri("https://management.core.windows.net/"
                                    + subscription.GetStringValue()
                                    + "/services/"
                                    + operation);
        }

        public static void SetInstanceCount(Subscription subscription, CloudService cloudService, Deployment deployment, int instanceCount)
        {
            XmlDocument config = GetDeploymentConfiguration(subscription, cloudService, deployment);
            config.GetElementsByTagName("Instances")[0].Attributes["count"].Value = instanceCount.ToString();

            string operation = string.Format("hostedservices/{0}/deploymentslots/{1}/?comp=config", cloudService.GetStringValue(), deployment.GetStringValue());
            Uri requestUri = GetRequestUri(subscription, operation);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Headers.Add("x-ms-version", "2010-10-28");
            request.Method = "POST";
            request.ContentType = "application/xml";

            var cert = GetCertificate(subscription);
            if (cert != null)
            {
                request.ClientCertificates.Add(GetCertificate(subscription));
                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.Write("<ChangeConfiguration xmlns=\"http://schemas.microsoft.com/windowsazure\">");
                    writer.Write("<Configuration>");
                    var base64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(config.OuterXml));
                    writer.Write(base64);
                    writer.Write("</Configuration>");
                    writer.Write("</ChangeConfiguration>");
                }

                try
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse) request.GetResponse();

                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string response = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception e)
                {
                    Tracer.Trace(TraceMessage.Warning(e.Message));
                }
            }
        }

        private static XmlDocument GetDeployment(Subscription subscription, CloudService cloudService, Deployment deployment)
        {
            string operation = string.Format("hostedservices/{0}/deploymentslots/{1}", cloudService.GetStringValue(), deployment.GetStringValue());
            Uri requestUri = GetRequestUri(subscription, operation);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Headers.Add("x-ms-version", "2010-10-28");
            request.Method = "GET";
            request.ContentType = "application/xml";
            request.ClientCertificates.Add(GetCertificate(subscription));

            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string response = reader.ReadToEnd();
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(response);
                        return xmlDocument;
                    }
                }
            }
            catch (WebException e)
            {
                if (((HttpWebResponse) e.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    return null;   
                }
                throw;
            }
        }

        private static XmlDocument GetDeploymentConfiguration(Subscription subscription, CloudService cloudService,
                                                       Deployment deployment)
        {
            XmlDocument deploymentXml = GetDeployment(subscription, cloudService, deployment);
            if (deploymentXml != null)
            {
                var configs = deploymentXml.GetElementsByTagName("Configuration");
                if (configs.Count > 0)
                {
                    var configElement = configs[0];
                    var configBase64 = configElement.InnerText;
                    byte[] configByteArray = Convert.FromBase64String(configBase64);
                    string config = Encoding.ASCII.GetString(configByteArray);
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(config);
                    return xmlDocument;
                }
            }
            return null;
        }

        public static int GetInstanceCount(Subscription subscription, CloudService cloudService, Deployment deployment)
        {
            XmlDocument config = GetDeploymentConfiguration(subscription, cloudService, deployment);
            return config == null ? 0 : Convert.ToInt32(config.GetElementsByTagName("Instances")[0].Attributes["count"].Value);
        }
    }
}
