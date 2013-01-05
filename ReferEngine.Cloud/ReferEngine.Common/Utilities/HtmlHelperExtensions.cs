using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ReferEngine.Common.Utilities
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string path, string alt = "", string className = null,
            string id = null)
        {
            var tagBuilder = new TagBuilder("img");

            if (className != null)
            {
                tagBuilder.AddCssClass(className);
            }

            if (id != null)
            {
                tagBuilder.GenerateId(id);
            }

            tagBuilder.Attributes.Add(new KeyValuePair<string, string>("alt", alt));

            string extension = Path.GetExtension(path);
            extension = extension != null ? extension.Substring(1) : null;
            string data = Util.GetImageBase64String(path);
            string src = string.Format("data:image/{0};base64,{1}", extension, data);
            tagBuilder.Attributes.Add(new KeyValuePair<string, string>("src", src));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
