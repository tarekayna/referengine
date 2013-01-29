using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ReferEngine.Workers.Pinger
{
    public class WorkerRole : RoleEntryPoint
    {
        private static IList<string> _websitesToPing;
        private const long BluGraphingCalculatorAppId = 21;
        private readonly TimeSpan _sleepTimeBetweenIterations = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _sleepTimeBetweenSites = TimeSpan.FromSeconds(1);

        public override void Run()
        {
            while (true)
            {
                foreach (string website in _websitesToPing)
                {
                    try
                    {
                        HttpWebRequest httpWebRequest = WebRequest.CreateHttp(website);
                        httpWebRequest.GetResponse();
                    }
                    catch (Exception)
                    {
                        // This is a best effort role. Move on
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

            _websitesToPing = new List<string>
                {
                    "https://www.referengine.com",
                    "https://www.referengine.com/account/login",
                    "https://www.referengine.com/pricing",
                    "https://www.referengine.com/contact",
                    string.Format("https://www.referengine.com/fb/app/{0}", BluGraphingCalculatorAppId),
                    string.Format("https://www.referengine.com/recommend/win8/intro/{0}", BluGraphingCalculatorAppId)
                };

            return base.OnStart();
        }
    }
}
