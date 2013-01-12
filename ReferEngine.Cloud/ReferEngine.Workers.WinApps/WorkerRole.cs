using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Workers.WinApps
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            while (true)
            {
                string url = "http://apps.microsoft.com/windows/sitemap/sitemap_1.xml";
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(url);
                XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("loc");
                List<AppWebLink> appWebLinks = new List<AppWebLink>();
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlNode xmlNode = xmlNodeList.Item(i);
                    AppWebLink appWebLink = new AppWebLink()
                        {
                            Link = xmlNode.InnerText,
                            LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(30))
                        };
                    appWebLinks.Add(appWebLink);
                }
                DatabaseOperations.AddAppWebLinks(appWebLinks);

                Thread.Sleep(TimeSpan.FromHours(23));
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
