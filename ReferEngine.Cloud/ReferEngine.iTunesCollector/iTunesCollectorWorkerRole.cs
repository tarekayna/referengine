using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.iOS;
using ReferEngine.Common.Models.iOS;
using ReferEngine.Common.Tracing;
using ReferEngine.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ReferEngine.iTunesCollector
{
    public class iTunesCollectorWorkerRole : RoleEntryPoint
    {
        private readonly TimeSpan _sleepBetweenRuns = TimeSpan.FromHours(1);
        private const string EpfPassword = "cd33da6220b9786a4a08a59758a3a7d6";
        private const string EpfUserId = "r3f3r3ng1n3";
        private const string EpfBaseLink = "https://feeds.itunes.apple.com/feeds/epf/v3/full/current/";
        private const string iTunesFileNameFormat = "itunes{0}.tbz";
        private const string iTunesFilePathFormat = EpfBaseLink + iTunesFileNameFormat;
        private const string PricingFileNameFormat = "pricing{0}.tbz";
        private const string PricingFilePathFormat = EpfBaseLink + PricingFileNameFormat;
        private const string PopularityFileNameFormat = "popularity{0}.tbz";
        private const string PopularityFilePathFormat = EpfBaseLink + PopularityFileNameFormat;
        private static readonly IList<string> FilesToDelete = new List<string>();
        private const char RecordSeparator = (char)2;
        
        // Current Max for Batch Size is 256kb 
        // http://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.queueclient.sendbatch.aspx
        private const int MaximumServiceBusBatchSize = 128*1024;

        private const string iTunesFilesToExtract = "application application_detail application_device_type " +
                                                    "artist artist_type artist_application device_type genre " +
                                                    "genre_application media_type storefront";
        private const string PopularityFilesToExtract = "application_popularity_per_genre";
        private const string PricingFilesToExtract = "application_price";

        public override void Run()
        {
            while (true)
            {
                try
                {
                    Tracer.Trace(TraceMessage.Info("Hi! Full import of iTunes data will start in 3.. 2.. 1.. go!"));

                    iOSDataImport lastDataImport = null;
                    if (Util.CurrentServiceConfiguration != Util.ReferEngineServiceConfiguration.Local)
                    {
                        lastDataImport = iOSDataReader.GetLastDataImport();   
                    }

                    string pathToLocalDirectory = RoleEnvironment.GetLocalResource("localStore").RootPath;

                    DateTime dateTime = DateTime.UtcNow;
                    string dateTimeString = dateTime.ToString("yyyyMMdd");
                    string iTunesRemoteFilePath = string.Format(iTunesFilePathFormat, dateTimeString);
                    string popularityRemoteFilePath = string.Format(PopularityFilePathFormat, dateTimeString);
                    string pricingRemoteFilePath = string.Format(PricingFilePathFormat, dateTimeString);

                    while (true)
                    {
                        if (RemoteFileExists(iTunesRemoteFilePath)) break;
                        dateTime = dateTime.Subtract(TimeSpan.FromDays(1));
                        dateTimeString = dateTime.ToString("yyyyMMdd");
                        iTunesRemoteFilePath = string.Format(iTunesFilePathFormat, dateTimeString);
                        popularityRemoteFilePath = string.Format(PopularityFilePathFormat, dateTimeString);
                        pricingRemoteFilePath = string.Format(PricingFilePathFormat, dateTimeString);
                    }

                    bool newFullExportExists = lastDataImport == null || lastDataImport.DateString != dateTimeString;
                    if (newFullExportExists)
                    {
                        iOSDataImport newDataImport = new iOSDataImport
                            {
                                DateString = dateTimeString,
                                StartTime = DateTime.UtcNow,
                                IsFullImport = true
                            };

                        string baseDirectory = pathToLocalDirectory;
                        //string baseDirectory = @"F:\Apple EPF";
                        //string baseDirectory = @"C:\Users\tarek\Documents";

                        string iTunesLocalFilePath = baseDirectory + string.Format(iTunesFileNameFormat, dateTimeString);
                        string popularityLocalFilePath = baseDirectory + string.Format(PopularityFileNameFormat, dateTimeString);
                        string pricingLocalFilePath = baseDirectory + string.Format(PricingFileNameFormat, dateTimeString);

                        DownloadFile(iTunesRemoteFilePath, iTunesLocalFilePath);
                        ExtractFilesToDirectory(iTunesLocalFilePath, pathToLocalDirectory, iTunesFilesToExtract);

                        ProcessRecordsFile(baseDirectory, "media_type", iOSServiceBusOperations.MediaTypesQueue);
                        ProcessRecordsFile(baseDirectory, "device_type", iOSServiceBusOperations.DeviceTypesQueue);
                        ProcessRecordsFile(baseDirectory, "genre", iOSServiceBusOperations.GenresQueue);
                        ProcessRecordsFile(baseDirectory, "storefront", iOSServiceBusOperations.StorefrontsQueue);
                        ProcessRecordsFile(baseDirectory, "artist_type", iOSServiceBusOperations.ArtistTypesQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.MediaTypesQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.DeviceTypesQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.GenresQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.StorefrontsQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.ArtistTypesQueue);

                        ProcessRecordsFile(baseDirectory, "artist", iOSServiceBusOperations.ArtistsQueue);
                        ProcessRecordsFile(baseDirectory, "application", iOSServiceBusOperations.AppsQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.ArtistsQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.AppsQueue);

                        ProcessRecordsFile(baseDirectory, "application_detail", iOSServiceBusOperations.AppDetailsQueue);
                        ProcessRecordsFile(baseDirectory, "application_device_type", iOSServiceBusOperations.ApplicationDeviceTypesQueue);
                        ProcessRecordsFile(baseDirectory, "artist_application", iOSServiceBusOperations.ArtistApplicationsQueue);
                        ProcessRecordsFile(baseDirectory, "genre_application", iOSServiceBusOperations.GenreApplicationsQueue);

                        DownloadFile(popularityRemoteFilePath, popularityLocalFilePath);
                        ExtractFilesToDirectory(popularityLocalFilePath, pathToLocalDirectory, PopularityFilesToExtract);
                        ProcessRecordsFile(baseDirectory, "application_popularity_per_genre", iOSServiceBusOperations.ApplicationPopularityPerGenreQueue);

                        DownloadFile(pricingRemoteFilePath, pricingLocalFilePath);
                        ExtractFilesToDirectory(pricingLocalFilePath, pathToLocalDirectory, PricingFilesToExtract);
                        ProcessRecordsFile(baseDirectory, "application_price", iOSServiceBusOperations.ApplicationPriceQueue);

                        WaitTillQueueIsEmpty(iOSServiceBusOperations.AppDetailsQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.ApplicationDeviceTypesQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.ArtistApplicationsQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.GenreApplicationsQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.ApplicationPopularityPerGenreQueue);
                        WaitTillQueueIsEmpty(iOSServiceBusOperations.ApplicationPriceQueue);

                        newDataImport.EndTime = DateTime.UtcNow;
                        iOSDatabaseWriter.AddNewDataImport(newDataImport);
                        Tracer.Trace(TraceMessage.Info("Finished Data Import."));
                    }
                }
                catch (Exception e)
                {
                    Tracer.Trace(TraceMessage.Exception(e));
                    throw;
                }
                finally
                {
                    foreach (string filePath in FilesToDelete.Where(File.Exists))
                    {
                        File.Delete(filePath);
                    }
                }

                Tracer.Trace(TraceMessage.Info("Done! Good night."));
                Thread.Sleep(_sleepBetweenRuns);
            }
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns

        private static void ExtractFilesToDirectory(string filePath, string directoryPath, string filesToExtract)
        {
            if (filesToExtract.Split(' ').All(x => File.Exists(directoryPath + "\\" + x))) return;
            
            Tracer.Trace(TraceMessage.Info("Start extracting " + filePath));
            Process process = new Process
                {
                    StartInfo =
                        {
                            UseShellExecute = false,
                            FileName = AppDomain.CurrentDomain.BaseDirectory + @"\7z\7z.exe",
                            Arguments = string.Format("e -y -o\"{0}\" \"{1}\"", directoryPath, filePath)
                        }
                };
            process.Start();
            process.WaitForExit();

            string tarFilePath = filePath.Substring(0, filePath.Length - 3) + "tar";
            process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = AppDomain.CurrentDomain.BaseDirectory + @"\7z\7z.exe",
                    Arguments = string.Format("e -y -r -o\"{0}\" \"{1}\" {2}", directoryPath, tarFilePath, filesToExtract)
                }
            };
            process.Start();
            process.WaitForExit();

            FilesToDelete.Add(filePath);
            FilesToDelete.Add(tarFilePath);

            Tracer.Trace(TraceMessage.Info("Done extracting " + filePath));
        }

        private static void DownloadFile(string remoteFilePath, string localFilePath)
        {
            if (File.Exists(localFilePath)) return;
            WebClient webClient = new WebClient {Credentials = new NetworkCredential(EpfUserId, EpfPassword)};
            Tracer.Trace(TraceMessage.Info("Start downloading: " + remoteFilePath));
            webClient.DownloadFile(remoteFilePath, localFilePath);
            Tracer.Trace(TraceMessage.Info("Finished downloading"));
        }

        private static bool IsComment(string line)
        {
            return line.StartsWith("#") && line.EndsWith(RecordSeparator.ToString(CultureInfo.InvariantCulture));
        }

        private static void ProcessRecordsFile(string baseDirectory, string fileName, Queue queue)
        {
            string filePath = baseDirectory + "\\" + fileName;
            Tracer.Trace(TraceMessage.Info("Start: " + fileName));

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                var unqueuedRecords = new List<string>();
                var unqueuedRecordsSize = 0;
                var numberOfQueuedRecords = 0;
                string currentRecord = string.Empty;
   
                while (streamReader.Peek() >= 0)
                {
                    currentRecord += streamReader.ReadLine();
                    if (string.IsNullOrEmpty(currentRecord)) continue;
                    if (IsComment(currentRecord))
                    {
                        currentRecord = string.Empty;
                        continue;
                    }
                    if (!currentRecord.EndsWith(RecordSeparator.ToString(CultureInfo.InvariantCulture)))
                        continue;
                    currentRecord = currentRecord.Trim(RecordSeparator);
                    currentRecord = currentRecord.Trim();
                    int currentRecordByteSize = currentRecord.Length*sizeof (char);

                    if (unqueuedRecordsSize + currentRecordByteSize >= MaximumServiceBusBatchSize)
                    {
                        numberOfQueuedRecords += unqueuedRecords.Count();
                        queue.EnqueueBatch(unqueuedRecords);
                        unqueuedRecords.Clear();
                        unqueuedRecordsSize = 0;
                    }

                    unqueuedRecords.Add(currentRecord);
                    unqueuedRecordsSize += currentRecord.Length * sizeof(char);

                    currentRecord = string.Empty;
                }

                if (unqueuedRecords.Any())
                {
                    numberOfQueuedRecords += unqueuedRecords.Count();
                    queue.EnqueueBatch(unqueuedRecords);
                    unqueuedRecords.Clear();
                }

                Tracer.Trace(TraceMessage.Info(fileName + " #Queued Records: " + numberOfQueuedRecords.ToString("N")));
            }

            Tracer.Trace(TraceMessage.Info("End: " + fileName));
            FilesToDelete.Add(filePath);
        }

        private static void WaitTillQueueIsEmpty(Queue queue)
        {
            Tracer.Trace(TraceMessage.Info("Start WaitTillQueueIsEmpty: " + queue.Name));
            var queueMessageCount = queue.GetMessageCount();
            while (queueMessageCount > 0)
            {
                Thread.Sleep(TimeSpan.FromMinutes(5));
                queueMessageCount = queue.GetMessageCount();
                Tracer.Trace(TraceMessage.Info(queue.Name + " #Queue Length: " + queueMessageCount));
            }
            Tracer.Trace(TraceMessage.Info("End WaitTillQueueIsEmpty: " + queue.Name));
        }

        private static bool RemoteFileExists(string path)
        {
            try
            {
                var webRequest = WebRequest.Create(path);
                webRequest.Credentials = new NetworkCredential(EpfUserId, EpfPassword);
                webRequest.GetResponse();
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            iOSServiceBusOperations.Initialize();
            return base.OnStart();
        }
    }
}
