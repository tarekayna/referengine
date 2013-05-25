using System.Text;
using Microsoft.WindowsAzure.ServiceRuntime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.iOS;
using ReferEngine.Common.Models.iOS;
using ReferEngine.Common.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using ReferEngine.Common.Utilities;

namespace ReferEngine.iTunesCollector
{
    public class iTunesCollectorWorkerRole : RoleEntryPoint
    {
        #region Private Variables
        private readonly TimeSpan _sleepBetweenRuns = TimeSpan.FromHours(1);
        private const string EpfPassword = "cd33da6220b9786a4a08a59758a3a7d6";
        private const string EpfUserId = "r3f3r3ng1n3";
        private const string EpfBaseLink = "https://feeds.itunes.apple.com/feeds/epf/v3/full/current/";
        private const string EpfIncrementalBaseLink = EpfBaseLink + "incremental/{0}/";

        private const string iTunesFileNameFormat = "itunes{0}.tbz";
        private const string iTunesFilePathFormat = EpfBaseLink + iTunesFileNameFormat;
        private const string iTunesIncrementalFilePathFormat = EpfIncrementalBaseLink + iTunesFileNameFormat;

        private const string PricingFileNameFormat = "pricing{0}.tbz";
        private const string PricingFilePathFormat = EpfBaseLink + PricingFileNameFormat;
        private const string PricingIncrementalFilePathFormat = EpfIncrementalBaseLink + PricingFileNameFormat;

        private const string PopularityFileNameFormat = "popularity{0}.tbz";
        private const string PopularityFilePathFormat = EpfBaseLink + PopularityFileNameFormat;
        private const string PopularityIncrementalFilePathFormat = EpfIncrementalBaseLink + PopularityFileNameFormat;

        private static readonly IList<string> FilesToDelete = new List<string>();
        private const char RecordSeparator = (char)2;
        
        // Current Max for Batch Size is 128kb 
        // 5% for transmission addition
        // http://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.queueclient.sendbatch.aspx
        private const int MaximumServiceBusBatchSize = 128*1024*95/100;
        private const int MaximumServiceBusMessageSize = 64*1024*95/100;

        private const string iTunesFilesToExtract = "application application_detail application_device_type " +
                                                    "artist artist_type artist_application device_type genre " +
                                                    "genre_application media_type storefront";
        private const string PopularityFilesToExtract = "application_popularity_per_genre";
        private const string PricingFilesToExtract = "application_price";
        #endregion Private Variables

        public override void Run()
        {
            while (true)
            {
                try
                {
                    Tracer.Trace(TraceMessage.Info("Good morning!"));

                    var prop = GetImport();
                    if (prop == null)
                    {
                        Tracer.Trace(TraceMessage.Info("No new imports found. Sleeping"));
                        Thread.Sleep(TimeSpan.FromMinutes(30));
                        continue;
                    }

                    Tracer.Trace(TraceMessage.Info("Starting Import")
                                             .AddProperty("DateString", prop.DateString)
                                             .AddProperty("ImportType", prop.ImportType.GetStringValue()));

                    prop.BaseDirectory = RoleEnvironment.GetLocalResource("localStore").RootPath;
                    //prop.BaseDirectory = @"F:\Apple EPF";
                    //prop.BaseDirectory = @"C:\Users\tarek\Documents";

                    string iTunesLocalFilePath = prop.BaseDirectory + string.Format(iTunesFileNameFormat, prop.DateString);
                    DownloadFile(prop.iTunesRemoteFilePath, iTunesLocalFilePath);
                    ExtractFilesToDirectory(iTunesLocalFilePath, prop.BaseDirectory, iTunesFilesToExtract);

                    var mediaTypeStep = StartProcessRecordsFileStep(prop, ImportStepName.MediaTypes, "media_type", iOSServiceBusOperations.MediaTypesQueue);
                    var deviceTypeStep = StartProcessRecordsFileStep(prop, ImportStepName.DeviceTypes, "device_type", iOSServiceBusOperations.DeviceTypesQueue);
                    var genreStep = StartProcessRecordsFileStep(prop, ImportStepName.Genres, "genre", iOSServiceBusOperations.GenresQueue);
                    var storefrontStep = StartProcessRecordsFileStep(prop, ImportStepName.Storefronts, "storefront", iOSServiceBusOperations.StorefrontsQueue);
                    var artistTypeStep = StartProcessRecordsFileStep(prop, ImportStepName.ArtistTypes, "artist_type", iOSServiceBusOperations.ArtistTypesQueue);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.MediaTypesQueue, mediaTypeStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.DeviceTypesQueue, deviceTypeStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.GenresQueue, genreStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.StorefrontsQueue, storefrontStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.ArtistTypesQueue, artistTypeStep);

                    var artistStep = StartProcessRecordsFileStep(prop, ImportStepName.Artists, "artist", iOSServiceBusOperations.ArtistsQueue);
                    var appStep = StartProcessRecordsFileStep(prop, ImportStepName.Apps, "application", iOSServiceBusOperations.AppsQueue);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.ArtistsQueue, artistStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.AppsQueue, appStep);

                    var appDetailStep = StartProcessRecordsFileStep(prop, ImportStepName.AppDetails, "application_detail", iOSServiceBusOperations.AppDetailsQueue);
                    var appDeviceTypeStep = StartProcessRecordsFileStep(prop, ImportStepName.AppDeviceTypes, "application_device_type", iOSServiceBusOperations.ApplicationDeviceTypesQueue);
                    var appArtistStep = StartProcessRecordsFileStep(prop, ImportStepName.AppArtists, "artist_application", iOSServiceBusOperations.ArtistApplicationsQueue);
                    var appGenreStep = StartProcessRecordsFileStep(prop, ImportStepName.AppGenres, "genre_application", iOSServiceBusOperations.GenreApplicationsQueue);
                    
                    string popularityLocalFilePath = prop.BaseDirectory + string.Format(PopularityFileNameFormat, prop.DateString);
                    DownloadFile(prop.PopularityRemoteFilePath, popularityLocalFilePath);
                    ExtractFilesToDirectory(popularityLocalFilePath, prop.BaseDirectory, PopularityFilesToExtract);
                   var appPopularityStep = StartProcessRecordsFileStep(prop, ImportStepName.AppPopularityPerGenres, "application_popularity_per_genre", iOSServiceBusOperations.ApplicationPopularityPerGenreQueue);
                    
                    string pricingLocalFilePath = prop.BaseDirectory + string.Format(PricingFileNameFormat, prop.DateString);
                    DownloadFile(prop.PricingRemoteFilePath, pricingLocalFilePath);
                    ExtractFilesToDirectory(pricingLocalFilePath, prop.BaseDirectory, PricingFilesToExtract);
                   var appPriceStep = StartProcessRecordsFileStep(prop, ImportStepName.AppPrice, "application_price", iOSServiceBusOperations.ApplicationPriceQueue);

                    WaitTillQueueIsEmpty(iOSServiceBusOperations.AppDetailsQueue, appDetailStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.ApplicationDeviceTypesQueue, appDeviceTypeStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.ArtistApplicationsQueue, appArtistStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.GenreApplicationsQueue, appGenreStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.ApplicationPopularityPerGenreQueue, appPopularityStep);
                    WaitTillQueueIsEmpty(iOSServiceBusOperations.ApplicationPriceQueue, appPriceStep);

                    iOSDatabaseWriter.AddDataImportStep(new iOSDataImportStep
                        {
                            DateString = prop.DateString,
                            EndTime = DateTime.UtcNow,
                            StartTime = DateTime.UtcNow,
                            ImportType = prop.ImportType,
                            Name = ImportStepName.Finished
                        });

                    Tracer.Trace(TraceMessage.Info("Finished Data Import."));
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

        private static ImportProperties GetImport()
        {
            DateTime dateTime = DateTime.UtcNow;
            var dateString = dateTime.ToString("yyyyMMdd");
            ImportProperties prop = new ImportProperties
            {
                DateString = dateString,
                iTunesRemoteFilePath = string.Format(iTunesFilePathFormat, dateString),
                PopularityRemoteFilePath = string.Format(PopularityFilePathFormat, dateString),
                PricingRemoteFilePath = string.Format(PricingFilePathFormat, dateString),
                ImportType = ImportType.Full
            };

            while (true)
            {
                if (RemoteFileExists(prop.iTunesRemoteFilePath)) break;

                dateTime = dateTime.Subtract(TimeSpan.FromDays(1));
                prop.DateString = dateTime.ToString("yyyyMMdd");
                prop.iTunesRemoteFilePath = string.Format(iTunesFilePathFormat, prop.DateString);
                prop.PopularityRemoteFilePath = string.Format(PopularityFilePathFormat, prop.DateString);
                prop.PricingRemoteFilePath = string.Format(PricingFilePathFormat, prop.DateString);
            }

            // prop = latest available full import
            var finishedStep = iOSDataReader.GetDataImportStep(prop.ImportType, prop.DateString, ImportStepName.Finished);
            if (finishedStep == null) return prop;

            // latest available full has already been finished
            // let's check for incremental imports after the latest full

            prop.ImportType = ImportType.Incremental;

            while (dateTime < DateTime.UtcNow)
            {
                dateTime = dateTime.AddDays(1);
                prop.DateString = dateTime.ToString("yyyyMMdd");
                prop.iTunesRemoteFilePath = string.Format(iTunesIncrementalFilePathFormat, prop.DateString);
                prop.PopularityRemoteFilePath = string.Format(PopularityIncrementalFilePathFormat, prop.DateString);
                prop.PricingRemoteFilePath = string.Format(PricingIncrementalFilePathFormat, prop.DateString);

                if (RemoteFileExists(prop.iTunesRemoteFilePath))
                {
                    finishedStep = iOSDataReader.GetDataImportStep(prop.ImportType, prop.DateString, ImportStepName.Finished);
                    if (finishedStep == null) return prop;
                }
            }

            return null;
        }

        private static iOSDataImportStep StartProcessRecordsFileStep(ImportProperties prop, ImportStepName stepName, string fileName, Queue queue)
        {
            var step = iOSDataReader.GetDataImportStep(prop.ImportType, prop.DateString, stepName);
            if (step != null)
            {
                string message = string.Format("Step already finished: {0}", stepName.GetStringValue());
                Tracer.Trace(TraceMessage.Info(message));
                return null;
            }
            Tracer.Trace(TraceMessage.Info(string.Format("Start: {0}", stepName.GetStringValue())));
            step = new iOSDataImportStep
            {
                StartTime = DateTime.UtcNow,
                ImportType = prop.ImportType,
                Name = stepName,
                DateString = prop.DateString
            };
            ProcessRecordsFile(prop.BaseDirectory, fileName, queue);
            return step;
        }

        private static void StepFinished(iOSDataImportStep step)
        {
            step.EndTime = DateTime.UtcNow;
            iOSDatabaseWriter.AddDataImportStep(step);
            Tracer.Trace(TraceMessage.Info(string.Format("Finished: {0}", step.Name.GetStringValue())));
        }

        private static void ProcessRecordsFile(string baseDirectory, string fileName, Queue queue)
        {
            string filePath = baseDirectory + "\\" + fileName;
            
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                var unqueuedBatch = new List<string>();
                var unqueuedBatchSize = 0;
                var unqueuedMessage = string.Empty;
                var unqueuedMessageSize = 0;
                var numberOfMessages = 0;
                var numberOfRecords = 0;
                var currentRecord = string.Empty;

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
                    currentRecord = currentRecord.Trim();
                    int currentRecordByteSize = Encoding.UTF8.GetByteCount(currentRecord);

                    if (unqueuedMessageSize + currentRecordByteSize >= MaximumServiceBusMessageSize)
                    {
                        if (unqueuedBatchSize + unqueuedMessageSize >= MaximumServiceBusBatchSize)
                        {
                            numberOfMessages += unqueuedBatch.Count();
                            queue.EnqueueBatch(unqueuedBatch);
                            unqueuedBatch.Clear();
                            unqueuedBatchSize = 0;
                        }

                        unqueuedBatch.Add(unqueuedMessage.Trim(RecordSeparator));
                        unqueuedBatchSize += unqueuedMessageSize;
                        unqueuedMessage = string.Empty;
                        unqueuedMessageSize = 0;
                    }

                    unqueuedMessage += currentRecord;
                    unqueuedMessageSize += currentRecordByteSize;
                    numberOfRecords++;
                    currentRecord = string.Empty;
                }

                if (unqueuedBatch.Any())
                {
                    numberOfMessages += unqueuedBatch.Count();
                    queue.EnqueueBatch(unqueuedBatch);
                    unqueuedBatch.Clear();
                }
                if (!string.IsNullOrEmpty(unqueuedMessage))
                {
                    queue.Enqueue(unqueuedMessage);
                }

                Tracer.Trace(TraceMessage.Info(fileName + " number of messages: " + numberOfMessages.ToString("N")));
                Tracer.Trace(TraceMessage.Info(fileName + " number of records: " + numberOfRecords.ToString("N")));
            }

            FilesToDelete.Add(filePath);
        }

        private static void DownloadFile(string remoteFilePath, string localFilePath)
        {
            if (File.Exists(localFilePath)) return;
            WebClient webClient = new WebClient {Credentials = new NetworkCredential(EpfUserId, EpfPassword)};
            Tracer.Trace(TraceMessage.Info("Start downloading: " + remoteFilePath));
            webClient.DownloadFile(remoteFilePath, localFilePath);
            Tracer.Trace(TraceMessage.Info("Finished downloading"));
        }

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

        private static bool IsComment(string line)
        {
            return line.StartsWith("#") && line.EndsWith(RecordSeparator.ToString(CultureInfo.InvariantCulture));
        }

        private static void WaitTillQueueIsEmpty(Queue queue, iOSDataImportStep step)
        {
            if (step == null) return;
            Tracer.Trace(TraceMessage.Info("Start - Wait till queue is empty: " + queue.Name));
            while (queue.HasMessages()) Thread.Sleep(TimeSpan.FromMinutes(1));
            StepFinished(step);
            Tracer.Trace(TraceMessage.Info("End - Wait till queue is empty: " + queue.Name));
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

    // ReSharper restore FunctionNeverReturns
}
