﻿using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.Resources
{
    public class ImageTag : Control
    {
        private readonly TagBuilder _tagBuilder;
        private readonly CloudinaryImage _cloudinaryImage;

        public ImageTag()
        {
            _tagBuilder = new TagBuilder("img");
        }

        public ImageTag(string cloudinaryId, string cloudinaryFormat, string transformation = null)
        {
            _tagBuilder = new TagBuilder("img");
            _cloudinaryImage = new CloudinaryImage { Id = cloudinaryId, Format = cloudinaryFormat };
            _tagBuilder.Attributes.Add("src", _cloudinaryImage.GetLink(transformation));
        }

        public ImageTag Attribute(string name, string value)
        {
            _tagBuilder.Attributes.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public ImageTag Alt(string value)
        {
            return Attribute("alt", value);
        }

        public ImageTag Height(int value)
        {
            return Attribute("height", value.ToString());
        }

        public ImageTag Width(int value)
        {
            return Attribute("width", value.ToString());
        }

        public IHtmlString ToHtmlString(string transformation = null)
        {
            if (!string.IsNullOrEmpty(transformation))
            {
                if (_tagBuilder.Attributes.Keys.Contains("src"))
                {
                    _tagBuilder.Attributes.Remove("src");
                }
                _tagBuilder.Attributes.Add("src", _cloudinaryImage.GetLink(transformation));
            }
            return new HtmlString(ToString());
        }

        public override string ToString()
        {
            return _tagBuilder.ToString();
        }
    }

    public static class Images
    {
        public static ImageTag LogoWebHeader { get { return new ImageTag("logo_web_header_oza9ph", "png"); } }
        public static ImageTag LogoMark{ get { return new ImageTag("logo_mark_500_qg2mad", "png"); }}
    }
}