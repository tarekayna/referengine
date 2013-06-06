using System;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.Models.iOS
{
    public enum ImportStepName
    {
        [StringValue("MediaTypes")]
        MediaTypes,

        [StringValue("DeviceTypes")]
        DeviceTypes,

        [StringValue("Genres")]
        Genres,

        [StringValue("Storefronts")]
        Storefronts,

        [StringValue("ArtistTypes")]
        ArtistTypes,

        [StringValue("Artists")]
        Artists,

        [StringValue("Apps")]
        Apps,

        [StringValue("AppDetails")]
        AppDetails,

        [StringValue("AppDeviceTypes")]
        AppDeviceTypes,

        [StringValue("AppArtists")]
        AppArtists,

        [StringValue("AppGenres")]
        AppGenres,

        [StringValue("AppPopularityPerGenres")]
        AppPopularityPerGenres,

        [StringValue("AppPrice")]
        AppPrice,

        [StringValue("RemoveOlderRecords")]
        RemoveOlderRecords,

        [StringValue("Finished")]
        Finished,
    }

    public enum ImportType
    {
        [StringValue("Incremental")]
        Incremental,

        [StringValue("Full")]
        Full
    }

    public class iOSDataImportStep
    {
        public int Id { get; set; }
        public string DateString { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ImportType ImportType { get; set; }
        public ImportStepName Name { get; set; }
    }
}
