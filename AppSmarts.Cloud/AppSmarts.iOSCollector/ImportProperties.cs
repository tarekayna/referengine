using AppSmarts.Common.Models.iOS;

namespace AppSmarts.iOSCollector
{
    internal class ImportProperties
    {
        public string iTunesRemoteFilePath { get; set; }
        public string PopularityRemoteFilePath { get; set; }
        public string PricingRemoteFilePath { get; set; }
        public string BaseDirectory { get; set; }
        public string DateString { get; set; }
        public ImportType ImportType { get; set; }
    }
}
