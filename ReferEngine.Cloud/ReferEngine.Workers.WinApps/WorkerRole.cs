using HtmlAgilityPack;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;

namespace ReferEngine.Workers.WinApps
{
    public class WorkerRole : RoleEntryPoint
    {
        private static bool ProcessStoreSitemap(string url)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(url);
                XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("loc");
                List<AppWebLink> appWebLinks = new List<AppWebLink>();
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlNode xmlNode = xmlNodeList.Item(i);
                    if (xmlNode != null)
                    {
                        string link = xmlNode.InnerText;
                        if (link.Contains("en-us") && !appWebLinks.Any(a => a.Link.Equals(link, StringComparison.OrdinalIgnoreCase)))
                        {
                            AppWebLink appWebLink = new AppWebLink
                                                        {
                                                            Link = xmlNode.InnerText,
                                                            LastUpdated = DateTime.Now
                                                        };
                            appWebLinks.Add(appWebLink);
                        }
                    }
                }
                DatabaseOperations.AddOrUpdateAppWebLinks(appWebLinks);
                return true;
            }
            catch (WebException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
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
                }

                // Now that we got all the links, time to scrape
                IList<AppWebLink> appWebLinks = DatabaseOperations.GetAppWebLinks();
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
                            string response = reader.ReadToEnd();
                            TextReader textReader = new StringReader(response);
                            HtmlDocument doc = new HtmlDocument();
                            doc.Load(textReader);

                            string msPageVer = GetAttributeValueOfXPathNode(doc, "//head/meta[@name='MS.PageVer']", "content");

                            if (msPageVer.Equals("1.0"))
                            {
                                StoreAppInfo storeAppInfo = new StoreAppInfo
                                {
                                    Name = GetInnerTextFromId(doc, "ProductTitleText"),
                                    Category = GetInnerTextFromId(doc, "CategoryText"),
                                    AgeRating = GetInnerTextFromId(doc, "AgeRating"),
                                    Developer = GetInnerTextFromId(doc, "AppDeveloper"),
                                    Copyright = GetInnerTextFromId(doc, "AppCopyrightText"),
                                    LogoLink = GetAttributeValueOfChildFromId(doc, "AppLogo", "img", "src"),
                                    DescriptionHtml = GetInnerTextFromId(doc, "DescriptionText"),
                                    FeaturesHtml = GetInnerHtmlFromId(doc, "FeatureText"),
                                    WebsiteLink = GetAttributeValueOfChildFromId(doc, "WebsiteLink", "a", "href"),
                                    SupportLink = GetAttributeValueOfChildFromId(doc, "SupportLink", "a", "href"),
                                    PrivacyPolicyLink = GetAttributeValueOfChildFromId(doc, "DevPrivacyPolicyLink", "a", "href"),
                                    ReleaseNotes = GetInnerTextFromId(doc, "ReleaseNotesText"),
                                    Architecture = GetInnerTextFromId(doc, "ArchitectureText"),
                                    Languages = GetInnerTextFromId(doc, "LanguagesText"),
                                    MsAppId = GetAttributeValueOfXPathNode(doc, "//head/meta[@name='MS.App.Id']", "content")
                                };

                                storeAppInfo.SetNumberOfRatings(GetInnerTextFromId(doc, "RatingText"));
                                storeAppInfo.SetRating(GetAttributeValueFromId(doc, "StarRating", "aria-label"));
                                storeAppInfo.SetPrice(GetInnerTextFromId(doc, "Price"));

                                storeAppInfo.PackageFamilyName = "";
                                HtmlNodeCollection scriptNodes = doc.DocumentNode.SelectNodes("//head/script");
                                foreach (var scriptNode in scriptNodes)
                                {
                                    if (scriptNode.InnerText.Contains("packageFamilyName ="))
                                    {
                                        int start = scriptNode.InnerText.IndexOf("packageFamilyName =",
                                                                                 StringComparison.OrdinalIgnoreCase);
                                        int packageStart = scriptNode.InnerText.IndexOf("'", start,
                                                                                        StringComparison
                                                                                            .OrdinalIgnoreCase);
                                        int packageEnd = scriptNode.InnerText.IndexOf("'", packageStart + 1,
                                                                                      StringComparison.OrdinalIgnoreCase);
                                        storeAppInfo.PackageFamilyName = scriptNode.InnerText.Substring(packageStart,
                                                                                           packageEnd - packageStart);
                                        break;
                                    }
                                }

                                DatabaseOperations.AddStoreAppInfo(storeAppInfo);
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
