using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Email;
using ReferEngine.Common.Models;
using System;
using System.Net;
using System.Threading;
using ReferEngine.Common.Tracing;
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
                        Tracer.Trace(new TraceMessage("Processing Message " + message.SequenceNumber, TraceMessageCategory.Info));

                        switch (message.ContentType)
                        {
                            case "ReferEngine.Common.Data.FacebookOperations":
                                {
                                    FacebookOperations facebookOperations = message.GetBody<FacebookOperations>();
                                    DataOperations.AddOrUpdatePerson(facebookOperations.GetCurrentUser());
                                    DataOperations.AddPersonAndFriends(facebookOperations.GetCurrentUser(), facebookOperations.GetFriends(), message);
                                    break;
                                }
                            case "ReferEngine.Common.Models.AppRecommendation":
                                {
                                    AppRecommendation appRecommendation = message.GetBody<AppRecommendation>();
                                    AppRecommendation.ProcessNew(appRecommendation);
                                    break;
                                }
                            case "ReferEngine.Common.Models.AppReceipt":
                                {
                                    AppReceipt appReceipt = message.GetBody<AppReceipt>();
                                    DataOperations.AddOrUpdateAppReceipt(appReceipt);
                                    break;
                                }
                            case "ReferEngine.Common.Models.PrivateBetaSignup":
                                {
                                    PrivateBetaSignup privateBetaSignup = message.GetBody<PrivateBetaSignup>();
                                    Emailer.ProcessPrivateBetaSignup(privateBetaSignup);
                                    DataOperations.AddPrivateBetaSignup(privateBetaSignup);
                                    break;
                                }
                            case "ReferEngine.Common.Models.FacebookPageViewInfo":
                                {
                                    FacebookPageViewInfo viewInfo = message.GetBody<FacebookPageViewInfo>();
                                    DataOperations.GetIpAddressLocation(viewInfo.IpAddress);

                                    long facebookPostId = 0;
                                    try
                                    {
                                        facebookPostId = Convert.ToInt64(viewInfo.ActionId);
                                    }
                                    catch (Exception e)
                                    {
                                        Tracer.Trace(TraceMessage.Exception(e));
                                    }

                                    AppRecommendation recommendation = DataOperations.GetAppRecommendation(facebookPostId);

                                    FacebookPageView pageView = new FacebookPageView
                                    {
                                        AppId = viewInfo.AppId,
                                        AppRecommendationFacebookPostId = recommendation != null ? recommendation.FacebookPostId : 0,
                                        TimeStamp = viewInfo.TimeStamp,
                                        IpAddress = viewInfo.IpAddress
                                    };
                                    DataOperations.AddFacebookPageView(pageView);
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
                    Tracer.Trace(TraceMessage.Exception(e));
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Tracer.Trace(TraceMessage.Exception(e));
                        throw;
                    }

                    Thread.Sleep(Settings.Default.WorkerThreadSleepTimeout);
                }
                catch (OperationCanceledException e)
                {
                    if (!IsStopped)
                    {
                        Tracer.Trace(TraceMessage.Exception(e));
                        throw;
                    }
                }
                catch (Exception e)
                {
                    Tracer.Trace(TraceMessage.Exception(e));
                }
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount;

            ServiceBusOperations.Initialize();

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
