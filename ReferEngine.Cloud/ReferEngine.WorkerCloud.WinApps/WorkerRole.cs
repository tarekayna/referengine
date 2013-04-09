using HtmlAgilityPack;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using ReferEngine.Common.Tracing;

namespace ReferEngine.WorkerCloud.WinApps
{
    public class WorkerRole : RoleEntryPoint
    {
        private static bool ProcessStoreSitemap(string url)
        {
            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(url);
            }
            catch (WebException)
            {
                return false;
            }

            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("loc");
            List<WindowsAppStoreLink> windowsAppStoreLinks = new List<WindowsAppStoreLink>();
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlNode xmlNode = xmlNodeList.Item(i);
                if (xmlNode == null) continue;
                string link = xmlNode.InnerText;
                if (link.Contains("en-us") && !windowsAppStoreLinks.Any(a => a.Link.Equals(link, StringComparison.OrdinalIgnoreCase)))
                {
                    WindowsAppStoreLink appWebLink = new WindowsAppStoreLink
                    {
                        Link = xmlNode.InnerText,
                        LastUpdated = DateTime.UtcNow,
                        NumberOfConsecutiveFailures = 0
                    };
                    windowsAppStoreLinks.Add(appWebLink);
                }
            }

            try
            {
                DataOperations.AddOrUpdateWindowsAppStoreLinks(windowsAppStoreLinks);
            }
            catch (SqlException)
            {
            }
            return true;
        }

        private static string GetInnerTextFromId(HtmlDocument doc, string id)
        {
            HtmlNode node = doc.GetElementbyId(id);
            return node != null ? node.InnerText : "";
        }

        private static string GetAttributeValueFromId(HtmlDocument doc, string id, string attributeName)
        {
            HtmlNode node = doc.GetElementbyId(id);
            return node != null ? node.GetAttributeValue(attributeName, "") : "";
        }

        private static string GetAttributeValueOfChildFromId(HtmlDocument doc, string id, string childName, string attributeName)
        {
            HtmlNode node = doc.GetElementbyId(id);
            if (node != null)
            {
                HtmlNode childNode = node.ChildNodes.FindFirst(childName);
                if (childNode != null)
                {
                    return childNode.GetAttributeValue(attributeName, "");
                }
            }

            return "";
        }

        private static string GetInnerHtmlFromId(HtmlDocument doc, string id)
        {
            HtmlNode node = doc.GetElementbyId(id);
            return node != null ? node.InnerHtml : "";
        }

        private static string GetAttributeValueOfXPathNode(HtmlDocument doc, string xPath, string attributeName)
        {
            HtmlNode node = doc.DocumentNode.SelectSingleNode(xPath);
            return node != null ? node.GetAttributeValue(attributeName, "") : "";
        }

        public override void Run()
        {
            while (true)
            {
                Tracer.Trace(TraceMessage.Info("Resume WinAppsWorker"));

                DateTime startTime = DateTime.UtcNow;
                int numberOfLinks = 0;
                int numberOfLiveLinks = 0;
                int numberOfDownLinks = 0;
                int numberOfDeletedLinks = 0;
                int numberOfNewApps = 0;
                int numberOfUpdatedApps = 0;

                try
                {
                    const string appStoreSiteMap = "http://apps.microsoft.com/windows/sitemap/sitemap_{0}.xml";
                    int sitemapIndex = 1;
                    string url = string.Format(appStoreSiteMap, sitemapIndex);
                    //while (ProcessStoreSitemap(url))
                    //{
                    //    Tracer.Trace(TraceMessage.Info("ProcessStoreSitemap").AddProperty("url", url));
                    //    sitemapIndex++;
                    //    url = string.Format(appStoreSiteMap, sitemapIndex);
                    //}

                    // Now that we got all the links, time to scrape
                    //IList<WindowsAppStoreLink> windowsAppStoreLinks = DataOperations.GetWindowsAppStoreLinks();

                    IList<WindowsAppStoreLink> windowsAppStoreLinks = new List<WindowsAppStoreLink>();

                    windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-us/app/wow-engineer/1586f767-2ae2-4b1b-a9bb-543714486652" });
                    windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-us/app/vedic-patri/621e45bd-a207-4a7f-a93a-58ed48b3a821/m/ROW" });
                    windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-us/app/windows-media-player-8/6ead7ee1-3f23-46e4-b60b-06605e09ed43/m/ROW" });
                    windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-us/app/trendwatcher/7d6e52bc-2bc3-428e-b4fe-74d015b04f6e/m/ROW" });
                    windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-us/app/wow-herbalist/8eb2ba67-2a03-42b1-9022-d029c10ae144" });
                    windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-US/app/blu-graphing-calculator/764cce31-8f93-48a6-b4fc-008eb78e50d4" });
                    windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-US/app/skype/5e19cc61-8994-4797-bdc7-c21263f6282b" });

                    numberOfLinks = windowsAppStoreLinks.Count();

                    for (int i = 0; i < windowsAppStoreLinks.Count; i++)
                    {
                        var windowsAppStoreLink = windowsAppStoreLinks.ElementAt(i);

                        if (i%1000 == 0 && i != 0)
                        {
                            Tracer.Trace(TraceMessage.Info("Requesting WindowsAppStoreLinks #" + i));
                        }

                        HttpWebRequest httpWebRequest = WebRequest.CreateHttp(windowsAppStoreLink.Link);
                        HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                        if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                        {
                            numberOfLiveLinks++;
                            Stream stream = httpWebResponse.GetResponseStream();
                            if (stream != null)
                            {
                                StreamReader reader = new StreamReader(stream);
                                string response = string.Empty;
                                try
                                {
                                    response = reader.ReadToEnd();
                                }
                                catch (IOException e)
                                {
                                    Console.WriteLine(e);
                                }
                                if (!string.IsNullOrEmpty(response))
                                {
                                    TextReader textReader = new StringReader(response);
                                    HtmlDocument doc = new HtmlDocument();
                                    doc.Load(textReader);

                                    string msPageVer = GetAttributeValueOfXPathNode(doc,
                                                                                    "//head/meta[@name='MS.PageVer']",
                                                                                    "content");

                                    if (msPageVer.Equals("1.0") && doc.GetElementbyId("ErrorPanel") == null)
                                    {
                                        string logoLink = GetAttributeValueOfChildFromId(doc, "AppLogo", "img", "src");
                                        string category = GetInnerTextFromId(doc, "CategoryText");
                                        string dev = GetInnerTextFromId(doc, "AppDeveloper");
                                        dev = dev.Replace("Publisher: ", "").Trim();

                                        var storeAppInfo = new WindowsAppStoreInfo
                                        {
                                            Name = GetInnerTextFromId(doc, "ProductTitleText"),
                                            AppStoreLink = windowsAppStoreLink.Link,
                                            AgeRating = GetInnerTextFromId(doc, "AgeRating"),
                                            Developer = dev,
                                            Copyright = GetInnerTextFromId(doc, "AppCopyrightText"),
                                            DescriptionHtml = GetInnerHtmlFromId(doc, "DescriptionText"),
                                            FeaturesHtml = GetInnerHtmlFromId(doc, "FeatureText"),
                                            WebsiteLink = GetAttributeValueOfChildFromId( doc, "WebsiteLink", "a", "href"),
                                            SupportLink = GetAttributeValueOfChildFromId( doc, "SupportLink", "a", "href"),
                                            PrivacyPolicyLink = GetAttributeValueOfChildFromId(doc, "DevPrivacyPolicyLink", "a", "href"),
                                            ReleaseNotes = GetInnerTextFromId(doc, "ReleaseNotesText"),
                                            Architecture = GetInnerTextFromId(doc, "ArchitectureText"),
                                            Languages = GetInnerTextFromId(doc, "LanguagesText"),
                                            MsAppId = GetAttributeValueOfXPathNode(doc, "//head/meta[@name='MS.App.Id']", "content")
                                        };

                                        storeAppInfo.SetNumberOfRatings(GetInnerTextFromId(doc, "RatingText"));
                                        storeAppInfo.SetRating(GetAttributeValueFromId(doc, "StarRating", "aria-label"));
                                        storeAppInfo.SetPrice(GetInnerTextFromId(doc, "Price"));

                                        HtmlNodeCollection scriptNodes = doc.DocumentNode.SelectNodes("//head/script");
                                        foreach (var scriptNode in scriptNodes)
                                        {
                                            if (scriptNode.InnerText.Contains("packageFamilyName ="))
                                            {
                                                int start = scriptNode.InnerText.IndexOf("packageFamilyName =",
                                                                                         StringComparison
                                                                                             .OrdinalIgnoreCase);
                                                int packageStart = scriptNode.InnerText.IndexOf("'", start,
                                                                                                StringComparison
                                                                                                    .OrdinalIgnoreCase);
                                                int packageEnd = scriptNode.InnerText.IndexOf("'", packageStart + 1,
                                                                                              StringComparison
                                                                                                  .OrdinalIgnoreCase);
                                                storeAppInfo.PackageFamilyName = scriptNode.InnerText.Substring(
                                                    packageStart + 1,
                                                    packageEnd - packageStart - 1);
                                                break;
                                            }
                                        }

                                        var styleNodes = doc.DocumentNode.SelectNodes("//head/style");
                                        foreach (HtmlNode styleNode in styleNodes)
                                        {
                                            int startSearchFrom = styleNode.InnerText.IndexOf(".appColors", StringComparison.Ordinal);
                                            if (startSearchFrom != -1)
                                            {
                                                int s = styleNode.InnerText.IndexOf("background-color", startSearchFrom, StringComparison.Ordinal);
                                                int hashIndex = styleNode.InnerText.IndexOf("#", s, StringComparison.Ordinal);
                                                storeAppInfo.BackgroundColor = styleNode.InnerText.Substring(hashIndex,
                                                                                                             7);
                                            }
                                        }

                                        List<ImageInfo> images = new List<ImageInfo>();
                                        HtmlNode node = doc.GetElementbyId("ScreenshotImageButtons");
                                        if (node != null)
                                        {
                                            var screenshotInfo = node.ChildNodes.Where(
                                                n =>
                                                n.Name == "a" &&
                                                n.Attributes.Any(
                                                    a => a.Name == "class" && a.Value.Contains("imageButton")))
                                                                     .Select(
                                                                         n =>
                                                                         new
                                                                             {
                                                                                 url =
                                                                             n.GetAttributeValue("imgurl", null),
                                                                                 caption =
                                                                             n.GetAttributeValue("imgcaption", null)
                                                                             });
                                            foreach (dynamic s in screenshotInfo)
                                            {
                                                if (!string.IsNullOrEmpty(s.url))
                                                {
                                                    images.Add(new ImageInfo { Description = s.caption, Link = s.url });
                                                }
                                            }
                                        }

                                        try
                                        {
                                            bool isNew = DataOperations.AddOrUpdateWindowsAppStoreInfo(storeAppInfo, category, logoLink, images);
                                            if (isNew)
                                            {
                                                numberOfNewApps++;
                                            }
                                            else
                                            {
                                                numberOfUpdatedApps++;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Tracer.Trace(TraceMessage.Exception(e).AddProperty("WindowsAppStoreLink", windowsAppStoreLink.Link));
                                        }
                                    }
                                    else
                                    {
                                        numberOfDownLinks++;
                                        windowsAppStoreLink.NumberOfConsecutiveFailures++;
                                        if (windowsAppStoreLink.NumberOfConsecutiveFailures == 5)
                                        {
                                            DataOperations.DeleteWindowsAppStoreLink(windowsAppStoreLink);
                                            numberOfDeletedLinks++;
                                        }
                                        else
                                        {
                                            DataOperations.UpdateWindowsAppStoreLink(windowsAppStoreLink);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            numberOfDownLinks++;
                            windowsAppStoreLink.NumberOfConsecutiveFailures++;
                            if (windowsAppStoreLink.NumberOfConsecutiveFailures == 5)
                            {
                                DataOperations.DeleteWindowsAppStoreLink(windowsAppStoreLink);
                                numberOfDeletedLinks++;
                            }
                            else
                            {
                                DataOperations.UpdateWindowsAppStoreLink(windowsAppStoreLink);   
                            }
                        }

                        httpWebResponse.Close();
                    }
                }
                catch (Exception e)
                {
                    Tracer.Trace(TraceMessage.Exception(e));
                }

                Tracer.Trace(TraceMessage.Success("WinAppsWorker Finished")
                                         .AddProperty("Number of Links", numberOfLinks)
                                         .AddProperty("Start Time", startTime.ToShortTimeString())
                                         .AddProperty("End Time", DateTime.UtcNow.ToShortTimeString())
                                         .AddProperty("Number of Live Links", numberOfLiveLinks)
                                         .AddProperty("Number of Down Links", numberOfDownLinks)
                                         .AddProperty("Number of Deleted Links", numberOfDeletedLinks)
                                         .AddProperty("Number of Newly Added Apps", numberOfNewApps)
                                         .AddProperty("Number of Updated Apps", numberOfUpdatedApps));

                TimeSpan sleepTime = TimeSpan.FromHours(24).Subtract(DateTime.UtcNow.Subtract(startTime));
                Thread.Sleep(sleepTime);
            }
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns

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
