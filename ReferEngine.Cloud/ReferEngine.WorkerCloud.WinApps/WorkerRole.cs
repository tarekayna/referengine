using HtmlAgilityPack;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Email;
using ReferEngine.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

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
                Trace.TraceInformation("Resume WinAppsWorker");
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
                    while (ProcessStoreSitemap(url))
                    {
                        sitemapIndex++;
                        url = string.Format(appStoreSiteMap, sitemapIndex);
                    }

                    // Now that we got all the links, time to scrape
                    IList<WindowsAppStoreLink> windowsAppStoreLinks = DataOperations.GetWindowsAppStoreLinks();

                    //IList<WindowsAppStoreLink> windowsAppStoreLinks = new List<WindowsAppStoreLink>();
                    //windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-US/app/blu-graphing-calculator/764cce31-8f93-48a6-b4fc-008eb78e50d4" });
                    //windowsAppStoreLinks.Add(new WindowsAppStoreLink { Link = "http://apps.microsoft.com/windows/en-US/app/skype/5e19cc61-8994-4797-bdc7-c21263f6282b" });

                    numberOfLinks = windowsAppStoreLinks.Count();

                    foreach (var windowsAppStoreLink in windowsAppStoreLinks)
                    {
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
                                            Emailer.SendExceptionEmail(e, "WinAppsWorker Exception: " + storeAppInfo.MsAppId);
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
                    Emailer.SendExceptionEmail(e, "WinApps Worker Exception");
                }

                StringBuilder body = new StringBuilder();
                body.AppendLine("مرحبا");
                body.AppendLine();
                body.AppendLine("هيدا تقرير من عامل تجميع تطبيقات الويندوز");
                body.AppendLine();
                body.AppendLine("Start time: " + startTime.ToShortTimeString());
                body.AppendLine("End time: " + DateTime.UtcNow.ToShortTimeString());
                body.AppendLine();
                body.AppendLine("Number of Links: " + numberOfLinks);
                body.AppendLine("    - Live:      " + numberOfLiveLinks);
                body.AppendLine("    - Down:      " + numberOfDownLinks);
                body.AppendLine("    - Deleted:   " + numberOfDeletedLinks);
                body.AppendLine();
                body.AppendLine("Number of New Apps: " + numberOfNewApps);
                body.AppendLine("Number of Updated Apps: " + numberOfUpdatedApps);
                body.AppendLine();
                body.AppendLine(".نحنا بأمرك أستاذ");
                Emailer.SendEmail("tarek@referengine.com", "Good news everyone!", body.ToString());

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
