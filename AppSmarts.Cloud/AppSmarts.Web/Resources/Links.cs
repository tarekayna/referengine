using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace AppSmarts.Web.Resources
{
    public class Link
    {
        private readonly TagBuilder _tagBuilder;
        public Link(string relativeUrl, string innerHtml)
        {
            _tagBuilder = new TagBuilder("a") { InnerHtml = innerHtml };
            _tagBuilder.Attributes.Add(new KeyValuePair<string, string>("href", VirtualPathUtility.ToAbsolute(relativeUrl)));
        }

        public IHtmlString ToHtmlString()
        {
            return new HtmlString(_tagBuilder.ToString());
        }
    }

    public static class Links
    {
        private static IHtmlString GenerateNewLink(string href, string innerHtml)
        {
            return new Link(href, innerHtml).ToHtmlString();
        }

        public static IHtmlString About(string innerHtml)
        {
            return GenerateNewLink("~/about", innerHtml);
        }

        public static IHtmlString Developer(string innerHtml)
        {
            return GenerateNewLink("~/developer", innerHtml);
        }

        public static IHtmlString Pricing(string innerHtml)
        {
            return GenerateNewLink("~/developer/pricing", innerHtml);
        }

        public static IHtmlString WindowsAppStore(string innerHtml)
        {
            return GenerateNewLink("~/app-store/windows", innerHtml);
        }

        public static IHtmlString Home(string innerHtml)
        {
            return GenerateNewLink("~/", innerHtml);
        }

        public static IHtmlString PrivacyPolicy(string innerHtml)
        {
            return GenerateNewLink("~/about/privacy", innerHtml);
        }

        public static IHtmlString TermsOfUse(string innerHtml)
        {
            return GenerateNewLink("~/about/terms", innerHtml);
        }

        public static IHtmlString AcceptableUsePolicy(string innerHtml)
        {
            return GenerateNewLink("~/about/use", innerHtml);
        }

        public static IHtmlString Copyright(string innerHtml)
        {
            return GenerateNewLink("~/about/copyright", innerHtml);
        }

        public static IHtmlString Contact(string innerHtml)
        {
            return GenerateNewLink("~/about/contact", innerHtml);
        }
    }
}