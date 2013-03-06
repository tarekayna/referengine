using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Email;
using ReferEngine.Common.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using ReferEngine.Common.Utilities;
using ReferEngine.Workers.DataWriter.Properties;

namespace ReferEngine.Workers.DataWriter
{
    public class WorkerRole : RoleEntryPoint
    {
        public bool IsStopped { get; set; }

        public override void Run()
        {
            while (!IsStopped)
            {
                try
                {
                    BrokeredMessage message = ServiceBusOperations.GetMessage();
                    if (message == null)
                    {
                        Thread.Sleep(Settings.Default.WorkerThreadSleepTimeout);
                    }
                    else
                    {
                        Trace.WriteLine("Processing", message.SequenceNumber.ToString());

                        switch (message.ContentType)
                        {
                            case "ReferEngine.Common.Data.FacebookOperations":
                                {
                                    FacebookOperations facebookOperations = message.GetBody<FacebookOperations>();
                                    DatabaseOperations.AddOrUpdatePerson(facebookOperations.GetCurrentUser());
                                    DatabaseOperations.AddPersonAndFriends(facebookOperations.GetCurrentUser(), facebookOperations.GetFriends(), message);
                                    break;
                                }
                            case "ReferEngine.Common.Models.AppRecommendation":
                                {
                                    AppRecommendation appRecommendation = message.GetBody<AppRecommendation>();
                                    DatabaseOperations.AddRecommendation(appRecommendation);

                                    App app = DatabaseOperations.GetApp(appRecommendation.AppId);
                                    Person person = DatabaseOperations.GetPerson(appRecommendation.PersonFacebookId);
                                    ReferEmailer.SendRecommendationThankYouEmail(app, person);
                                    break;
                                }
                            case "ReferEngine.Common.Models.AppReceipt":
                                {
                                    AppReceipt appReceipt = message.GetBody<AppReceipt>();
                                    DatabaseOperations.AddOrUpdateAppReceipt(appReceipt);
                                    break;
                                }
                            case "ReferEngine.Common.Models.PrivateBetaSignup":
                                {
                                    PrivateBetaSignup privateBetaSignup = message.GetBody<PrivateBetaSignup>();
                                    ReferEmailer.ProcessPrivateBetaSignup(privateBetaSignup);
                                    DatabaseOperations.AddPrivateBetaSignup(privateBetaSignup);
                                    break;
                                }
                        }

                        message.Complete();
                    }
                }
                catch (MessageLockLostException e)
                {
                    // What to do? Message took too long.
                    // It's ok, process it again.
                    Trace.WriteLine("MessageLockLostException: " + e.Message);
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }

                    Thread.Sleep(Settings.Default.WorkerThreadSleepTimeout);
                }
                catch (OperationCanceledException e)
                {
                    if (!IsStopped)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.Message, e);
                }
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount;

            ServiceBusOperations.Initialize();

            DiagnosticMonitorConfiguration diagnosticMonitorConfiguration = DiagnosticMonitor.GetDefaultInitialConfiguration();
            diagnosticMonitorConfiguration.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(5);
            diagnosticMonitorConfiguration.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            var diagnosticMonitor = DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticMonitorConfiguration);

            IsStopped = false;
            return base.OnStart();
        }

        public override void OnStop()
        {
            IsStopped = true;
            base.OnStop();
        }
    }
}
