using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.MixPanel;

namespace ReferEngine.Common.UnitTests.Data
{
    [TestClass]
    public class MixPanelOperationsTests
    {
        [TestMethod]
        public void TestBasicEventsRequest()
        {
            MixPanelRequestInfo requestInfo = new MixPanelRequestInfo(Event.RecommendIntro)
            {
                PropertyName = "AppId",
                PropertyValues = new List<string> { "21" }
            };
            MixPanelOperations.GetData(requestInfo);
        }
    }
}
