using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Data.MixPanel
{
    public enum EventUnit
    {
        [StringValue("minute")]
        Minute,

        [StringValue("hour")]
        Hour,

        [StringValue("day")]
        Day,

        [StringValue("week")]
        Week,

        [StringValue("month")]
        Month,
    }
}
