using System.Data.SqlClient;
using HtmlAgilityPack;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Email;
using ReferEngine.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using ReferEngine.Common.Utilities;

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
            List<WindowsAppStoreLink> appWebLinks = new List<WindowsAppStoreLink>();
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlNode xmlNode = xmlNodeList.Item(i);
                if (xmlNode == null) continue;
                string link = xmlNode.InnerText;
                if (link.Contains("en-us") && !appWebLinks.Any(a => a.Link.Equals(link, StringComparison.OrdinalIgnoreCase)))
                {
                    WindowsAppStoreLink appWebLink = new WindowsAppStoreLink
                    {
                        Link = xmlNode.InnerText,
                        LastUpdated = DateTime.UtcNow
                    };
                    appWebLinks.Add(appWebLink);
                }
            }

            try
            {
                DataOperations.AddOrUpdateAppWebLinks(appWebLinks);
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
                DateTime startTime = DateTime.Now;

                const string appStoreSiteMap = "http://apps.microsoft.com/windows/sitemap/sitemap_{0}.xml";
                int sitemapIndex = 1;
                string url = string.Format(appStoreSiteMap, sitemapIndex);

                while (ProcessStoreSitemap(url))
                {
                    sitemapIndex++;
                    url = string.Format(appStoreSiteMap, sitemapIndex);
                    //if (Util.CurrentServiceConfiguration == Util.ReferEngineServiceConfiguration.Local) break;
                }

                // Now that we got all the links, time to scrape
                IList<WindowsAppStoreLink> appWebLinks = new List<WindowsAppStoreLink>();

                try
                {
                    appWebLinks = DataOperations.GetWindowsAppStoreLinks();
                }
                catch (SqlException)
                {
                }

                foreach (var appWebLink in appWebLinks)
                {
                    HttpWebRequest httpWebRequest = WebRequest.CreateHttp(appWebLink.Link);
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
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
                                    WindowsAppStoreInfo storeAppInfo = new WindowsAppStoreInfo
                                    {
                                        Name = GetInnerTextFromId(doc, "ProductTitleText"),
                                        AppStoreLink = appWebLink.Link,
                                        Category = GetInnerTextFromId(doc, "CategoryText"),
                                        AgeRating = GetInnerTextFromId(doc, "AgeRating"),
                                        Developer = GetInnerTextFromId(doc, "AppDeveloper"),
                                        Copyright = GetInnerTextFromId(doc, "AppCopyrightText"),
                                        LogoLink = GetAttributeValueOfChildFromId(doc,
                                                                           "AppLogo",
                                                                           "img",
                                                                           "src"),
                                        DescriptionHtml = GetInnerTextFromId(doc,
                                                               "DescriptionText"),
                                        FeaturesHtml = GetInnerHtmlFromId(doc, "FeatureText"),
                                        WebsiteLink = GetAttributeValueOfChildFromId(doc,
                                                                           "WebsiteLink",
                                                                           "a",
                                                                           "href"),
                                        SupportLink = GetAttributeValueOfChildFromId(doc,
                                                                           "SupportLink",
                                                                           "a",
                                                                           "href"),
                                        PrivacyPolicyLink = GetAttributeValueOfChildFromId(doc,
                                                                           "DevPrivacyPolicyLink",
                                                                           "a",
                                                                           "href"),
                                        ReleaseNotes = GetInnerTextFromId(doc,
                                                               "ReleaseNotesText"),
                                        Architecture = GetInnerTextFromId(doc,
                                                               "ArchitectureText"),
                                        Languages = GetInnerTextFromId(doc, "LanguagesText"),
                                        MsAppId = GetAttributeValueOfXPathNode(doc,
                                                                         "//head/meta[@name='MS.App.Id']",
                                                                         "content")
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
                                        int startSearchFrom = styleNode.InnerText.IndexOf(".appColors");
                                        if (startSearchFrom != -1)
                                        {
                                            int s = styleNode.InnerText.IndexOf("background-color", startSearchFrom);
                                            int hashIndex = styleNode.InnerText.IndexOf("#", s);
                                            storeAppInfo.BackgroundColor = styleNode.InnerText.Substring(hashIndex, 7);
                                        }
                                    }

                                    // Screenshot Links
                                    List<WindowsAppStoreScreenshot> screenshots = new List<WindowsAppStoreScreenshot>();
                                    HtmlNode node = doc.GetElementbyId("ScreenshotImageButtons");
                                    if (node != null)
                                    {
                                        var screenshotInfo = node.ChildNodes.Where(n => n.Name == "a" && n.Attributes.Any(a => a.Name == "class" && a.Value.Contains("imageButton")))
                                            .Select(n => new { url = n.GetAttributeValue("imgurl", null), caption = n.GetAttributeValue("imgcaption", null) });
                                        foreach (dynamic s in screenshotInfo)
                                        {
                                            if (!string.IsNullOrEmpty(s.url))
                                            {
                                                var screenshot = new WindowsAppStoreScreenshot
                                                {
                                                    StoreAppInfoMsAppId = storeAppInfo.MsAppId,
                                                    Link = s.url,
                                                    Caption = s.caption
                                                };
                                                screenshots.Add(screenshot);
                                            }
                                        }
                                    }

                                    try
                                    {
                                        DataOperations.AddWindowsAppStoreInfo(storeAppInfo);
                                        foreach (WindowsAppStoreScreenshot storeAppScreenshot in screenshots)
                                        {
                                            DataOperations.AddWindowsAppStoreScreenshot(storeAppScreenshot);
                                        }
                                    }
                                    catch (SqlException e)
                                    {
                                        ReferEmailer.SendPlainTextEmail("tarek@referengine.com", "WinAppsWorker Exception", e.Message);
                                    }
                                }
                            }
                        }
                    }

                    httpWebResponse.Close();
                }

                TimeSpan sleepTime = TimeSpan.FromHours(24).Subtract(DateTime.Now.Subtract(startTime));
                Thread.Sleep(sleepTime);
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
