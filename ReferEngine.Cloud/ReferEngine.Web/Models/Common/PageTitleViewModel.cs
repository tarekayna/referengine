using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferEngine.Web.Models.Common
{
    public class PageTitleViewModel
    {
        public PageTitleViewModel(string pageTitle)
        {
            PageTitle = pageTitle;
        }

        public string PageTitle { get; set; }
    }
}