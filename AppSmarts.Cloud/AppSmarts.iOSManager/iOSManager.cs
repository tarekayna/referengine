using AppSmarts.Common.AzureManagement;
using AppSmarts.Common.Data.iOS;
using AppSmarts.Common.Tracing;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace appSmarts.iOSManager
{
    public class iOSManager : RoleEntryPoint
    {
        private static int MaxCruncherInstanceCount
        {
            get { return Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("MaxInstanceCount")); }
        }
        private static int MessagesPerInstance
        {
            get { return Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("MessagesPerInstance")); }
        }
        private static int SleepInMinutes
        {
            get { return Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("SleepInMinutes")); }
        }
        
        private static long GetNumberOfMessagesInAllQueues()
        {
            return iOSServiceBusOperations.Queues.Where(x => !x.IsDisabled()).Sum(queue => queue.GetMessageCount());
        }

        private static long _lastTotalNumberOfMessages;

        private static int GetSuggestedInstanceCount()
        {
            long numberOfMessages = GetNumberOfMessagesInAllQueues();
            if (numberOfMessages != _lastTotalNumberOfMessages)
            {
                Tracer.Trace(TraceMessage.Info("Total number of messages = " + numberOfMessages));
                _lastTotalNumberOfMessages = numberOfMessages;
            }
            var count = (int)(numberOfMessages / MessagesPerInstance);
            if (count < 1) return 1;
            return count > MaxCruncherInstanceCount ? MaxCruncherInstanceCount : count;
        }

        public override void Run()
        {
            while (true)
            {
                try
                {
                    var cruncherInstanceCount = AzureManager.GetInstanceCount(Subscription.PayAsYouGo,
                                                                              CloudService.iOSQueueCruncher,
                                                                              Deployment.Production);

                    if (cruncherInstanceCount > 0)
                    {
                        var suggestedInstanceCount = GetSuggestedInstanceCount();

                        if (cruncherInstanceCount != suggestedInstanceCount)
                        {
                            Tracer.Trace(TraceMessage.Info("Changing instance count to " + suggestedInstanceCount));
                            AzureManager.SetInstanceCount(Subscription.PayAsYouGo,
                                                          CloudService.iOSQueueCruncher,
                                                          Deployment.Production,
                                                          suggestedInstanceCount);
                        }
                    }
                }
                catch (Exception e)
                {
                    Tracer.Trace(TraceMessage.ExceptionWarning(e));
                }

                Thread.Sleep(TimeSpan.FromMinutes(SleepInMinutes));
            }
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            iOSServiceBusOperations.Initialize();
            return base.OnStart();
        }
    }
}
