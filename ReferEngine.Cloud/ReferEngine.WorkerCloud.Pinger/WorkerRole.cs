using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Tracing;
using ReferEngine.Common.Utilities;

namespace ReferEngine.WorkerCloud.Pinger
{
    public class WorkerRole : RoleEntryPoint
    {
        private static IList<string> _websitesToPing;
        private const long BluGraphingCalculatorAppId = 21;
        private readonly TimeSpan _sleepTimeBetweenIterations = TimeSpan.FromMinutes(15);
        private readonly TimeSpan _sleepTimeBetweenSites = TimeSpan.FromSeconds(1);

        public override void Run()
        {
            if (Util.CurrentServiceConfiguration != Util.ReferEngineServiceConfiguration.ProductionCloud) return;
            while (true)
            {
                foreach (string website in _websitesToPing)
                {
                    try
                    {
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

                Thread.Sleep(_sleepTimeBetweenIterations);
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            const string baseUrl = "https://www.referengine.com";
            //const string baseUrl = "http://127.0.0.1:81";

            _websitesToPing = new List<string>
                {
                    string.Format("{0}/", baseUrl),
                    //string.Format("{0}/fb/app/{1}", baseUrl, BluGraphingCalculatorAppId),
                    string.Format("{0}/recommend/win8/intro/{1}", baseUrl, BluGraphingCalculatorAppId),
                    string.Format("{0}/pricing", baseUrl),
                    string.Format("{0}/contact", baseUrl),
                    string.Format("{0}/account/login", baseUrl),
                    string.Format("{0}/account/register", baseUrl),
                    string.Format("{0}/about/privacy", baseUrl),
                    string.Format("{0}/about/terms", baseUrl),
                    string.Format("{0}/about/use", baseUrl),
                    string.Format("{0}/about/copyright", baseUrl),
                    string.Format("{0}/about/rules/{1}", baseUrl, BluGraphingCalculatorAppId),
                };

            return base.OnStart();
        }
    }
}
