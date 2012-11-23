using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferEngine.Models
{
    public enum AppPlatform
    {
        Windows8,
        iOS,
        Android
    }

    public class App
    {
        public int Id { get; set; }
        public string AppStoreLink { get; set; }
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public Developer Developer { get; set; }
        public AppPlatform Platform { get; set; }
    }
}