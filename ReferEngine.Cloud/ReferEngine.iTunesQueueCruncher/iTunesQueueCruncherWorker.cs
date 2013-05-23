using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.iOS;
using System.Net;
using System.Linq;
using ReferEngine.Common.Tracing;

namespace ReferEngine.iTunesQueueCruncher
{
    public class iTunesQueueCruncherWorker : RoleEntryPoint
    {
        private const char FieldSeparator = (char)1;
        private const int BatchSize = 5000;
        delegate void ProcessRecordsDelegateWithMessages(IEnumerable<IList<string>> unprocessedRecordsInfo, BrokeredMessage[] messages);

        private void ProcessMessagesInQueue(Queue queue, ProcessRecordsDelegateWithMessages processRecordsDelegate)
        {
            while (true)
            {
                var messages = queue.ReceiveBatch(BatchSize);
                if (messages == null) return;
                var messagesArray = messages.ToArray();
                if (!messagesArray.Any()) return;

                var records = messagesArray.Select(x => x.GetBody<string>().Split(FieldSeparator));
                processRecordsDelegate(records, messagesArray);

                for (int i = 0; i < messagesArray.Count(); i++)
                {
                    var lockedUntilUtc = messagesArray.ElementAt(i).LockedUntilUtc;
                    if (DateTime.UtcNow > lockedUntilUtc.Subtract(TimeSpan.FromSeconds(30)))
                    {
                        for (int j = i; j < messagesArray.Count(); j++)
                        {
                            messagesArray.ElementAt(j).RenewLock();
                        }
                    }
                    messagesArray.ElementAt(i).Complete();
                }
                Tracer.Trace(TraceMessage.Info(queue.Name + " processed " + messagesArray.Count()));
            }
        }

        public override void Run()
        {
            while (true)
            {
                try
                {
                    ProcessMessagesInQueue(iOSServiceBusOperations.MediaTypesQueue, iOSDatabaseWriter.AddOrUpdateMediaTypes);
                    ProcessMessagesInQueue(iOSServiceBusOperations.DeviceTypesQueue, iOSDatabaseWriter.AddOrUpdateDeviceTypes);
                    ProcessMessagesInQueue(iOSServiceBusOperations.StorefrontsQueue, iOSDatabaseWriter.AddOrUpdateStorefronts);
                    ProcessMessagesInQueue(iOSServiceBusOperations.GenresQueue, iOSDatabaseWriter.AddOrUpdateGenres);
                    ProcessMessagesInQueue(iOSServiceBusOperations.ArtistTypesQueue, iOSDatabaseWriter.AddOrUpdateArtistTypes);
                    ProcessMessagesInQueue(iOSServiceBusOperations.ArtistsQueue, iOSDatabaseWriter.AddOrUpdateArtists);
                    ProcessMessagesInQueue(iOSServiceBusOperations.AppsQueue, iOSDatabaseWriter.AddOrUpdateApps);
                    ProcessMessagesInQueue(iOSServiceBusOperations.AppDetailsQueue, iOSDatabaseWriter.AddOrUpdateAppDetails);
                    ProcessMessagesInQueue(iOSServiceBusOperations.ApplicationDeviceTypesQueue, iOSDatabaseWriter.AddOrUpdateAppDeviceTypes);
                    ProcessMessagesInQueue(iOSServiceBusOperations.ArtistApplicationsQueue, iOSDatabaseWriter.AddOrUpdateAppArtists);
                    ProcessMessagesInQueue(iOSServiceBusOperations.GenreApplicationsQueue, iOSDatabaseWriter.AddOrUpdateAppGenres);
                    ProcessMessagesInQueue(iOSServiceBusOperations.ApplicationPopularityPerGenreQueue, iOSDatabaseWriter.AddOrUpdateAppPopularityPerGenres);
                    ProcessMessagesInQueue(iOSServiceBusOperations.ApplicationPriceQueue, iOSDatabaseWriter.AddOrUpdateAppPrices);

                    Thread.Sleep(TimeSpan.FromMinutes(10));
                }
                catch (Exception exception)
                {
                    Tracer.Trace(TraceMessage.Exception(exception));
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            }
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            iOSServiceBusOperations.Initialize();
            return base.OnStart();
        }
    }
}
