using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.MixPanel;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.UnitTests.Data
{
    [TestClass]
    public class DatabaseOperationsTests
    {
        [TestMethod]
        public void TestGetAppDashboardViewModel()
        {
            App app = DatabaseOperations.GetApp(21);
            DatabaseOperations.GetAppDashboardViewModel(app);
        }
    }
}
