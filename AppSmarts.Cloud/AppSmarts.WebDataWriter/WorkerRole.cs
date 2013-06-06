using AppSmarts.Common.Data;
using AppSmarts.Common.Email;
using AppSmarts.Common.Models;
using AppSmarts.Common.Tracing;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Net;
using System.Threading;

namespace AppSmarts.WebDataWriter
{
    public class WorkerRole : RoleEntryPoint
    {
        public bool IsStopped { get; set; }

        private int _sleepTime = -1;

        private int SleepTime
        {
            get
            {
                if (_sleepTime == -1)
                {
                    _sleepTime = Convert.ToInt32(CloudConfigurationManager.GetSetting("SleepBetweenGetMessage"));
                }
                return _sleepTime;
            }
        }

        public override void Run()
        {
            while (!IsStopped)
            {
                try
                {
                    BrokeredMessage message = ServiceBusOperations.GetMessage();
                    if (message == null)
                    {
                        Thread.Sleep(SleepTime);
                    }
                    else
                    {
                        Tracer.Trace(new TraceMessage("Processing Message " + message.SequenceNumber, TraceMessageCategory.Info));

                        switch (message.ContentType)
                        {
                            case "ReferEngine.Common.Data.FacebookOperations":
                                {
                                    FacebookAccessSession facebookOperations = message.GetBody<FacebookAccessSession>();
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

                    Thread.Sleep(SleepTime);
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
