using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace ReferEngine.WorkerCloud.iOSApps
{
    public class WorkerRole : RoleEntryPoint
    {
        private static HtmlDocument GetHtmlDocumentFromLink(string link)
        {
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(link);
            HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream stream = httpWebResponse.GetResponseStream();
                if (stream != null)
                {
                    StreamReader reader = new StreamReader(stream);
                    string response = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(response))
                    {
                        TextReader textReader = new StringReader(response);
                        HtmlDocument doc = new HtmlDocument();
                        doc.Load(textReader);
                        return doc;
                    }
                }
            }
            return null;
        }

        public override void Run()
        {
            while (false)
            {
                const string link = "https://itunes.apple.com/us/genre/ios/id36?mt=8";
                HtmlDocument mainPageDocument = GetHtmlDocumentFromLink(link);
                //*[@id="genre-nav"]/div/ul[1]/li[1]/a
                //*[@id="genre-nav"]/div/ul[1]/li[2]/a

                IEnumerable<HtmlNode> categorieNodes = mainPageDocument.DocumentNode.Descendants()
                                .Where(
                                    n =>
                                    n.Name == "a" &&
                                    n.Attributes.Any(
                                        a => a.Name == "href" && a.Value.Contains("https://itunes.apple.com/us/genre/ios")));
                IEnumerable<string> categoryLinks  = categorieNodes.Select(n => n.GetAttributeValue("href", null));

                // categoryLink + "&letter=A&page=1";

                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
