using System.Collections.Generic;

namespace AppSmarts.Common.Data.MixPanel
{
    public class MixPanelRequestInfo
    {
        public MixPanelRequestInfo(Event mixPanelEvent)
        {
            AnalysisType = AnalysisType.General;
            EventUnit = EventUnit.Day;
            ReturnFormat = ReturnFormat.Json;
            Interval = 7;
            Event = mixPanelEvent;
        }

        public AnalysisType AnalysisType { get; set; }
        public EventUnit EventUnit { get; set; }
        public ReturnFormat ReturnFormat { get; set; }
        public Event Event { get; set; }
        public int Interval { get; set; }
        public string PropertyName { get; set; }
        public IList<string> PropertyValues { get; set; }
    }
}
