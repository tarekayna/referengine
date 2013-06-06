using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading;
using AppSmarts.Common.Data;
using AppSmarts.Common.Data.iOS;
using AppSmarts.Common.Tracing;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Net;
using System.Linq;

namespace AppSmarts.iOSQueueCruncher
{
    public class iOSQueueCruncher : RoleEntryPoint
    {
        private const char FieldSeparator = (char)1;
        private const char RecordSeparator = (char)2;
        private const int BatchSize = 500;
        delegate void ProcessRecordsDelegateWithMessages(IEnumerable<IList<string>> unprocessedRecordsInfo, BrokeredMessage[] messages);

        private void ProcessMessagesInQueue(Queue queue, ProcessRecordsDelegateWithMessages processRecordsDelegate)
        {
            while (true)
            {
                if (queue.IsDisabled() || queue.IsEmpty()) break;
                var batch = queue.ReceiveBatch(BatchSize);
                if (batch == null) break;
                var messages = batch.ToArray();
                if (!messages.Any()) break;

                Tracer.Trace(TraceMessage.Info(string.Format("Processing {0} messages in {1}", messages.Count(), queue.Name)));

                var records = new List<IList<string>>();
                foreach (var brokeredMessage in messages)
                {
                    var messageBody = brokeredMessage.GetBody<string>();
                    var messageRecords = messageBody.Split(RecordSeparator);
                    foreach (var messageRecord in messageRecords)
                    {
                        records.Add(messageRecord.Split(FieldSeparator));
                    }
                }

                processRecordsDelegate(records, messages);

                queue.CompleteBatch(messages);
                
                var msg = string.Format("{0} - Processed - Records: {1}, Messages: {2}", queue.Name,
                                        records.Count(), messages.Count());
                Tracer.Trace(TraceMessage.Info(msg));
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
                }
                catch(DbEntityValidationException exception)
                {
                    Tracer.Trace(TraceMessage.Exception(exception));
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
                catch (Exception exception)
                {
                    Tracer.Trace(TraceMessage.Exception(exception));
                    Thread.Sleep(TimeSpan.FromSeconds(30));
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
