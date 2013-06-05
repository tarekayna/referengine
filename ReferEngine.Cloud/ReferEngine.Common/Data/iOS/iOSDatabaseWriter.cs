using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using ReferEngine.Common.Models.iOS;
using System.Linq;
using ReferEngine.Common.Tracing;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Data.iOS
{
    public static class iOSDatabaseWriter
    {
        public static void AddOrUpdateMediaTypes(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);

                    var thisRecord = new iOSMediaType
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        Id = Convert.ToInt64(recordInfo[1]),
                        Name = recordInfo[2]
                    };

                    var databaseRecord = db.iOSMediaTypes.FirstOrDefault(x => x.Id == thisRecord.Id);
                    if (databaseRecord == null)
                    {
                        db.iOSMediaTypes.Add(thisRecord);
                    }
                    else
                    {
                        db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);
                    }
                }
                db.SaveChanges();
                return null;
            });
        }

        public static void AddOrUpdateDeviceTypes(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);

                    var thisRecord = new iOSDeviceType
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        Id = Convert.ToInt32(recordInfo[1]),
                        Name = recordInfo[2]
                    };

                    var databaseRecord = db.iOSDeviceTypes.FirstOrDefault(x => x.Id == thisRecord.Id);
                    if (databaseRecord == null)
                    {
                        db.iOSDeviceTypes.Add(thisRecord);
                    }
                    else
                    {
                        db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateGenres(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                var recordsArray = records.OrderBy(x => x[2]);
                foreach (IList<string> recordInfo in recordsArray)
                {
                    iOSServiceBusOperations.RenewLocks(messages);

                    iOSGenre parentGenre = null;
                    if (!string.IsNullOrEmpty(recordInfo[2]))
                    {
                        int parentGenreId = Convert.ToInt32(recordInfo[2]);
                        parentGenre = db.iOSGenres.FirstOrDefault(x => x.Id == parentGenreId);
                    }
                    var thisRecord = new iOSGenre
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        Id = Convert.ToInt32(recordInfo[1]),
                        ParentGenre = parentGenre,
                        Name = recordInfo[3]
                    };

                    var databaseRecord = db.iOSGenres.FirstOrDefault(x => x.Id == thisRecord.Id);
                    if (databaseRecord == null)
                    {
                        db.iOSGenres.Add(thisRecord);
                    }
                    else
                    {
                        db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);
                        databaseRecord.ParentGenre = thisRecord.ParentGenre;
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateStorefronts(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                

                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);

                    var thisRecord = new iOSStorefront
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        Id = Convert.ToInt32(recordInfo[1]),
                        CountryCode = recordInfo[2],
                        Name = recordInfo[3]
                    };

                    var databaseRecord = db.iOSStorefronts.FirstOrDefault(x => x.Id == thisRecord.Id);
                    if (databaseRecord == null)
                    {
                        db.iOSStorefronts.Add(thisRecord);
                    }
                    else
                    {
                        db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateArtists(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);

                    var thisRecord = new iOSArtist
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        Id = Convert.ToInt64(recordInfo[1]),
                        Name = recordInfo[2]
                    };
                    
                    if (recordInfo.Count() == 6)
                    {
                        thisRecord.IsActualArtist = recordInfo[3] == "1";
                        thisRecord.ViewUrl = recordInfo[4];
                        int artistTypeId = Convert.ToInt32(recordInfo[5]);
                        var artistType = db.iOSArtistTypes.FirstOrDefault(x => x.Id == artistTypeId);
                        if (IsNull(artistType, true) || artistTypeId != 7) continue;
                        thisRecord.ArtistType = artistType;
                    }
                    else
                    {
                        thisRecord.SearchTerms = recordInfo[3];
                        thisRecord.IsActualArtist = recordInfo[4] == "1";
                        thisRecord.ViewUrl = recordInfo[5];
                        int artistTypeId = Convert.ToInt32(recordInfo[6]);
                        if (artistTypeId != 7) continue;
                        thisRecord.ArtistType = db.iOSArtistTypes.First(x => x.Id == artistTypeId);
                    }

                    var databaseRecord = db.iOSArtists.FirstOrDefault(x => x.Id == thisRecord.Id);
                    if (databaseRecord == null)
                    {
                        db.iOSArtists.Add(thisRecord);
                    }
                    else
                    {
                        db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateApps(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                

                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);

                    try
                    {
                        Int64 downloadSize = string.IsNullOrEmpty(recordInfo[16]) ? -1 : Convert.ToInt64(recordInfo[16]);
                        var thisRecord = new iOSApp
                            {
                                ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                                Id = Convert.ToInt64(recordInfo[1]),
                                Title = recordInfo[2],
                                RecommendedAge = recordInfo[3],
                                ArtistName = recordInfo[4],
                                SellerName = recordInfo[5],
                                CompanyUrl = recordInfo[6],
                                SupportUrl = recordInfo[7],
                                ViewUrl = recordInfo[8],
                                // ArtworkLarge from recordInfo[9]
                                // ArtworkSmall from recordInfo[10]
                                iTunesReleaseDate = Convert.ToDateTime(recordInfo[11]),
                                Copyright = recordInfo[12],
                                Description = recordInfo[13],
                                Version = recordInfo[14],
                                iTunesVersion = recordInfo[15],
                                DownloadSize = downloadSize
                            };
                        if (!string.IsNullOrEmpty(recordInfo[9]))
                        {
                            string artworkLink = recordInfo[9];
                            string linkHash = CloudinaryImage.GetOriginalLinkHash(artworkLink);
                            thisRecord.ArtworkLarge = db.CloudinaryImages.FirstOrDefault(x => x.OriginalLinkHash == linkHash);
                            if (thisRecord.ArtworkLarge == null)
                            {
                                try
                                {
                                    thisRecord.ArtworkLarge = CloudinaryConnector.UploadRemoteImage(new ImageInfo {Link = artworkLink}, "iOS");
                                }
                                catch (Exception e)
                                {
                                    Tracer.Trace(TraceMessage.Exception(e));
                                    Tracer.Trace(TraceMessage.Info("UploadRemoteImage")
                                                             .AddProperty("App Title", thisRecord.Title)
                                                             .AddProperty("Image Link", artworkLink));
                                }
                            }
                        }
                        
                        var databaseRecord = db.iOSApps.Where(x => x.Id == thisRecord.Id).Include(x => x.ArtworkLarge).FirstOrDefault();
                        if (databaseRecord == null)
                        {
                            db.iOSApps.Add(thisRecord);
                        }
                        else
                        {
                            db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);

                            if (databaseRecord.ArtworkLarge == null)
                            {
                                databaseRecord.ArtworkLarge = thisRecord.ArtworkLarge;
                            }
                            else if (thisRecord.ArtworkLarge == null ||
                                     databaseRecord.ArtworkLarge.Id != thisRecord.ArtworkLarge.Id)
                            {
                                CloudinaryConnector.DeleteImage(databaseRecord.ArtworkLarge);
                                db.CloudinaryImages.Remove(databaseRecord.ArtworkLarge);
                                databaseRecord.ArtworkLarge = thisRecord.ArtworkLarge;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Tracer.Trace(TraceMessage.Exception(e));
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        private static AppScreenshot GetAppScreenshotFromLink(string link, iOSDatabaseContext db)
        {
            AppScreenshot appScreenshot = null;
            var linkHash = CloudinaryImage.GetOriginalLinkHash(link);
            var cloudinaryImage = db.CloudinaryImages.FirstOrDefault(x => x.OriginalLinkHash == linkHash);
            if (cloudinaryImage != null)
            {
                appScreenshot = db.AppScreenshots.FirstOrDefault(x => x.CloudinaryImage.Id == cloudinaryImage.Id);
            }
            if (appScreenshot == null)
            {
                var img = CloudinaryConnector.UploadRemoteImage(new ImageInfo {Link = link});
                if (img != null)
                {
                    img = db.CloudinaryImages.Add(img);
                    appScreenshot = db.AppScreenshots.Add(new AppScreenshot {CloudinaryImage = img});
                }
            }
            return appScreenshot;
        }

        public static void AddOrUpdateAppDetails(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                foreach (IList<string> recordInfo in records.Where(x => x[2].Equals("EN", StringComparison.OrdinalIgnoreCase)))
                {
                    iOSServiceBusOperations.RenewLocks(messages);

                    int appId = Convert.ToInt32(recordInfo[1]);

                    var app = db.iOSApps.FirstOrDefault(x => x.Id == appId);
                    if (app == null)
                    {
                        Tracer.Trace(TraceMessage.Error("App does not exist for this AppDetail. AppId = " + recordInfo[1]));
                        continue;
                    }

                    var thisRecord = new iOSAppDetail
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        App = app,
                        LanguageCode = recordInfo[2],
                        Title = recordInfo[3],
                        Description = recordInfo[4],
                        ReleaseNotes = recordInfo[5],
                        CompanyUrl = recordInfo[6],
                        SupportUrl = recordInfo[7]
                    };

                    IList<string> imageLinks = new List<string>();
                    for (int j = 8; j < 12; j++)
                    {
                        if (string.IsNullOrEmpty(recordInfo[j])) continue;
                        imageLinks.Add(recordInfo[j]);
                    }

                    var databaseRecord = db.iOSAppDetails.Include(x => x.AppScreenshots.Select(y => y.CloudinaryImage)).FirstOrDefault(x => x.App.Id == thisRecord.App.Id && 
                                                                                                        x.LanguageCode == thisRecord.LanguageCode);
                    if (databaseRecord == null)
                    {
                        foreach (var image in imageLinks)
                        {
                            thisRecord.AppScreenshots.Add(GetAppScreenshotFromLink(image, db));
                        }

                        db.iOSAppDetails.Add(thisRecord);
                    }
                    else
                    {
                        databaseRecord.CompanyUrl = thisRecord.CompanyUrl;
                        databaseRecord.Title = thisRecord.Title;
                        databaseRecord.Description = thisRecord.Description;
                        databaseRecord.ReleaseNotes = thisRecord.ReleaseNotes;
                        databaseRecord.SupportUrl = thisRecord.SupportUrl;
                        databaseRecord.ExportDate = thisRecord.ExportDate;

                        var newScreenshots = imageLinks.Where(x => databaseRecord.AppScreenshots.All(c => c.CloudinaryImage != null && c.CloudinaryImage.OriginalLink != x)).ToArray();
                        var removedScreenshots = databaseRecord.AppScreenshots.Where(s => s.CloudinaryImage != null && imageLinks.All(i => i != s.CloudinaryImage.OriginalLink)).ToArray();

                        foreach (AppScreenshot screenshot in removedScreenshots)
                        {
                            databaseRecord.AppScreenshots.Remove(screenshot);
                            if (screenshot.CloudinaryImage != null)
                            {
                                CloudinaryConnector.DeleteImage(screenshot.CloudinaryImage);
                                db.CloudinaryImages.Remove(screenshot.CloudinaryImage);
                            }
                        }

                        foreach (string newImage in newScreenshots)
                        {
                            CloudinaryImage cloudinaryImage = CloudinaryConnector.UploadRemoteImage(new ImageInfo { Link = newImage });
                            if (cloudinaryImage != null)
                            {
                                AppScreenshot appScreenshot = new AppScreenshot {CloudinaryImage = cloudinaryImage};
                                databaseRecord.AppScreenshots.Add(appScreenshot);
                            }
                        }
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateAppDeviceTypes(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);
                    int appId = Convert.ToInt32(recordInfo[1]);
                    int deviceTypeId = Convert.ToInt32(recordInfo[2]);
                    var databaseRecord = db.iOSAppDeviceTypes.FirstOrDefault(x => x.App.Id == appId && x.DeviceType.Id == deviceTypeId);
                    if (databaseRecord == null)
                    {
                        var app = db.iOSApps.FirstOrDefault(x => x.Id == appId);
                        if (IsNull(app)) continue;

                        var deviceType = db.iOSDeviceTypes.FirstOrDefault(x => x.Id == deviceTypeId);
                        if (IsNull(deviceType, true)) continue;
                        var newRecord = new iOSAppDeviceType
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = app,
                            DeviceType = deviceType
                        };
                        db.iOSAppDeviceTypes.Add(newRecord);
                    }
                    else
                    {
                        databaseRecord.ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]);
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateAppArtists(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            
            iOSDatabaseConnector.Execute(db =>
            {
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);
                    int artistId = Convert.ToInt32(recordInfo[1]);
                    int appId = Convert.ToInt32(recordInfo[2]);
                    var databaseRecord = db.iOSAppArtists.FirstOrDefault(x => x.App.Id == appId && x.Artist.Id == artistId);
                    if (databaseRecord == null)
                    {
                        var app = db.iOSApps.FirstOrDefault(x => x.Id == appId);
                        if (app == null)
                        {
                            Tracer.Trace(TraceMessage.Warning("Did not find app for AppArtist: " + appId));
                            continue;
                        }
                        var artist = db.iOSArtists.FirstOrDefault(x => x.Id == artistId);
                        if (artist == null)
                        {
                            Tracer.Trace(TraceMessage.Warning("Did not find artist for AppArtist: " + artistId));
                            continue;
                        }
                        var newRecord = new iOSAppArtist
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = app,
                            Artist = artist
                        };
                        db.iOSAppArtists.Add(newRecord);
                    }
                    else
                    {
                        databaseRecord.ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]);
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        private static bool IsNull(object obj, bool traceWarning = false)
        {
            if (obj == null)
            {
                if (traceWarning)
                {
                    StackTrace stackTrace = new StackTrace();
                    var traceMessage = TraceMessage.Warning("Something is null")
                                                   .AddProperty("StackTrace", stackTrace.ToString());
                    Tracer.Trace(traceMessage);
                }
                return true;
            }
            return false;
        }

        public static void AddOrUpdateAppGenres(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {   
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);
                    int genreId = Convert.ToInt32(recordInfo[1]);
                    int appId = Convert.ToInt32(recordInfo[2]);

                    var databaseRecord = db.iOSAppGenres.FirstOrDefault(x => x.App.Id == appId && x.Genre.Id == genreId);
                    if (databaseRecord == null)
                    {
                        var app = db.iOSApps.FirstOrDefault(x => x.Id == appId);
                        if (IsNull(app)) continue;

                        var genre = db.iOSGenres.FirstOrDefault(x => x.Id == genreId);
                        if (IsNull(genre, true)) continue;

                        var newRecord = new iOSAppGenre
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = app,
                            Genre = genre,
                            IsPrimary = recordInfo[3] == "1"
                        };
                        db.iOSAppGenres.Add(newRecord);
                    }
                    else
                    {
                        databaseRecord.ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]);
                        databaseRecord.IsPrimary = recordInfo[3] == "1";
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateArtistTypes(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);
                    int mediaTypeId = Convert.ToInt32(recordInfo[3]);
                    var thisRecord = new iOSArtistType
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        Id = Convert.ToInt32(recordInfo[1]),
                        Name = recordInfo[2],
                        MediaType = db.iOSMediaTypes.First(x => x.Id == mediaTypeId)
                    };

                    var databaseRecord = db.iOSArtistTypes.FirstOrDefault(x => x.Id == thisRecord.Id);
                    if (databaseRecord == null)
                    {
                        db.iOSArtistTypes.Add(thisRecord);
                    }
                    else
                    {
                        db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);
                        databaseRecord.MediaType = thisRecord.MediaType;
                    }
                }
                db.SaveChanges();
                return null;
            });
        }

        public static void AddOrUpdateAppPrices(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);
                    int appId = Convert.ToInt32(recordInfo[1]);
                    int storefrontId = Convert.ToInt32(recordInfo[4]);
                    var databaseRecord = db.iOSAppPrices.FirstOrDefault(x => x.App.Id == appId && x.Storefront.Id == storefrontId);
                    if (databaseRecord == null)
                    {
                        var app = db.iOSApps.FirstOrDefault(x => x.Id == appId);
                        if (IsNull(app)) continue;
                        var newRecord = new iOSAppPrice
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = app,
                            RetailPrice = recordInfo[2],
                            CurrencyCode = recordInfo[3]
                        };
                        db.iOSAppPrices.Add(newRecord);
                    }
                    else
                    {
                        databaseRecord.ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]);
                        databaseRecord.RetailPrice = recordInfo[2];
                        databaseRecord.CurrencyCode = recordInfo[3];
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddOrUpdateAppPopularityPerGenres(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                
                foreach (IList<string> recordInfo in records)
                {
                    iOSServiceBusOperations.RenewLocks(messages);
                    int storefrontId = Convert.ToInt32(recordInfo[1]);
                    int genreId = Convert.ToInt32(recordInfo[2]);
                    int appId = Convert.ToInt32(recordInfo[3]);
                    var databaseRecord = db.iOSAppPopularityPerGenres.FirstOrDefault(x => x.App.Id == appId && 
                                                        x.Storefront.Id == storefrontId && x.Genre.Id == genreId);
                    if (databaseRecord == null)
                    {
                        var app = db.iOSApps.FirstOrDefault(x => x.Id == appId);
                        if (IsNull(app)) continue;
                        var genre = db.iOSGenres.FirstOrDefault(x => x.Id == genreId);
                        if (IsNull(genre, true)) continue;
                        var storefront = db.iOSStorefronts.FirstOrDefault(x => x.Id == storefrontId);
                        if (IsNull(storefront, true)) continue;

                        var newRecord = new iOSAppPopularityPerGenre
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = app,
                            Genre = genre,
                            Storefront = storefront,
                            Rank = Convert.ToInt32(recordInfo[4])
                        };
                        db.iOSAppPopularityPerGenres.Add(newRecord);
                    }
                    else
                    {
                        databaseRecord.ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]);
                        databaseRecord.Rank = Convert.ToInt32(recordInfo[4]);
                    }
                }
                db.SaveChanges();

                return null;
            });
        }

        public static void AddDataImportStep(iOSDataImportStep dataImportStep)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                db.DataImportSteps.Add(dataImportStep);
                db.SaveChanges();
                return null;
            });
        }

        public static void RemoveOlderRecords(DateTime dateTime)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                var prices = db.iOSAppPrices.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in prices)
                {
                    db.iOSAppPrices.Remove(record);
                }
                db.SaveChanges();

                var pops = db.iOSAppPopularityPerGenres.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in pops)
                {
                    db.iOSAppPopularityPerGenres.Remove(record);
                }
                db.SaveChanges();

                var appGenres = db.iOSAppGenres.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in appGenres)
                {
                    db.iOSAppGenres.Remove(record);
                }
                db.SaveChanges();

                var appArtists = db.iOSAppArtists.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in appArtists)
                {
                    db.iOSAppArtists.Remove(record);
                }
                db.SaveChanges();

                var appDeviceTypes = db.iOSAppDeviceTypes.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in appDeviceTypes)
                {
                    db.iOSAppDeviceTypes.Remove(record);
                }
                db.SaveChanges();

                var appDetails = db.iOSAppDetails.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in appDetails)
                {
                    db.iOSAppDetails.Remove(record);
                }
                db.SaveChanges();

                var apps = db.iOSApps.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in apps)
                {
                    db.iOSApps.Remove(record);
                }
                db.SaveChanges();

                var artists = db.iOSArtists.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in artists)
                {
                    db.iOSArtists.Remove(record);
                }
                db.SaveChanges();

                var artistTypes = db.iOSArtistTypes.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in artistTypes)
                {
                    db.iOSArtistTypes.Remove(record);
                }
                db.SaveChanges();

                var storefronts = db.iOSStorefronts.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in storefronts)
                {
                    db.iOSStorefronts.Remove(record);
                }
                db.SaveChanges();

                var genres = db.iOSGenres.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in genres)
                {
                    db.iOSGenres.Remove(record);
                }
                db.SaveChanges();

                var deviceTypes = db.iOSDeviceTypes.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in deviceTypes)
                {
                    db.iOSDeviceTypes.Remove(record);
                }
                db.SaveChanges();

                var mediaTypes = db.iOSMediaTypes.Where(x => !x.ExportDate.IsSameDate(dateTime));
                foreach (var record in mediaTypes)
                {
                    db.iOSMediaTypes.Remove(record);
                }
                db.SaveChanges();

                return null;
            });
        }
    }
}
