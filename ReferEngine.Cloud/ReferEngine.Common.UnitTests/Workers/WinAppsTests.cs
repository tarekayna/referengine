using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferEngine.Workers.WinApps;

namespace ReferEngine.Common.UnitTests.Workers
{
    [TestClass]
    public class WinAppsTests
    {
        [TestMethod]
        public void TestWinAppsWorker()
        {
            var worker = new WorkerRole();
            worker.Run();
        }
    }
}
