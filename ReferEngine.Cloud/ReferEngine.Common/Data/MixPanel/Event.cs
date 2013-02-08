using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Data.MixPanel
{
    public enum Event
    {
        [StringValue("Recommend Intro")]
        RecommendIntro,

        [StringValue("Recommend Intro Start")]
        RecommendIntroStart,

        [StringValue("Recommend Intro Cancel")]
        RecommendIntroCancel,

        [StringValue("Recommend Post")]
        RecommendPost,

        [StringValue("Recommend Post Submit")]
        RecommendPostSubmit,

        [StringValue("Recommend Post Success")]
        RecommendPostSuccess,

        [StringValue("Recommend Post Cancel")]
        RecommendPostCancel,

        [StringValue("Recommend Post Error")]
        RecommendPostError,
    }
}
