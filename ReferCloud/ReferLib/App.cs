using System;

namespace ReferLib
{
    public class App
    {
        public Int64 Id { get; set; }
        public string AppStoreLink { get; set; }
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public AppPlatform Platform { get; set; }
        public Int64 DeveloperId { get; set; }
        public string Publisher { get; set; }
        public string Copyright { get; set; }
    }
}