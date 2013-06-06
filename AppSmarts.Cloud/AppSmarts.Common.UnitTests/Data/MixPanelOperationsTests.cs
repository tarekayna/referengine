using System.Collections.Generic;
using AppSmarts.Common.Data.MixPanel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSmarts.Common.UnitTests.Data
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
