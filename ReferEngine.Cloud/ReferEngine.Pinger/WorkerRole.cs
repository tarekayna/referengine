using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Tracing;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Pinger
{
    public class WorkerRole : RoleEntryPoint
    {
        private static IList<string> _websitesToPing;
        private const long BluGraphingCalculatorAppId = 21;
        private readonly TimeSpan _sleepTimeBetweenIterations = TimeSpan.FromMinutes(15);
        private readonly TimeSpan _sleepTimeBetweenSites = TimeSpan.FromSeconds(1);

        public override void Run()
        {
            while (true)
            {
                Tracer.Trace(TraceMessage.Info("Good morning.. pinger here.."));
                foreach (string website in _websitesToPing)
                {
                    try
                    {
                        Tracer.Trace(TraceMessage.Info(website));
                        HttpWebRequest httpWebRequest = WebRequest.CreateHttp(website);
                        httpWebRequest.GetResponse();
                    }
                    catch (Exception e)
                    {
                        Tracer.Trace(TraceMessage.Exception(e));
                    }
                    finally
                    {
                        Thread.Sleep(_sleepTimeBetweenSites);
                    }
                }

                Tracer.Trace(TraceMessage.Info("Sleeping, waking up in " + _sleepTimeBetweenIterations.TotalMinutes + " minutes."));
                Thread.Sleep(_sleepTimeBetweenIterations);
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 1;

            _websitesToPing = new List<string>
                {
                    string.Format("{0}", Util.BaseUrl),
                    string.Format("{0}app-store/windows", Util.BaseUrl),
                    string.Format("{0}developer", Util.BaseUrl),
                    string.Format("{0}recommend/win8/intro/{1}", Util.BaseUrl, BluGraphingCalculatorAppId),
                    string.Format("{0}contact", Util.BaseUrl),
                    string.Format("{0}account/login", Util.BaseUrl),
                    string.Format("{0}account/register", Util.BaseUrl),
                    string.Format("{0}about/privacy", Util.BaseUrl),
                    string.Format("{0}about/terms", Util.BaseUrl),
                    string.Format("{0}about/use", Util.BaseUrl),
                    string.Format("{0}about/copyright", Util.BaseUrl),
                    string.Format("{0}about/rules/{1}", Util.BaseUrl, BluGraphingCalculatorAppId),
                };

            return base.OnStart();
        }
    }
}
