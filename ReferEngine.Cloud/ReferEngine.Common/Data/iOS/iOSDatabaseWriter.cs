using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                var lockedUntilUtc = messages.First().LockedUntilUtc;

                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);

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
                var lockedUntilUtc = messages.First().LockedUntilUtc;

                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);

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
                var lockedUntilUtc = messages.First().LockedUntilUtc;

                var recordsArray = records.OrderBy(x => x[2]);
                foreach (IList<string> recordInfo in recordsArray)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);

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
                var lockedUntilUtc = messages.First().LockedUntilUtc;

                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);

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
                var lockedUntilUtc = messages.First().LockedUntilUtc;

                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);

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
                        if (artistTypeId != 7) continue;
                        thisRecord.ArtistType = db.iOSArtistTypes.First(x => x.Id == artistTypeId);
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
                var lockedUntilUtc = messages.First().LockedUntilUtc;

                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);

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
                            thisRecord.ArtworkLarge =
                                db.CloudinaryImages.FirstOrDefault(x => x.OriginalLink == artworkLink);
                            if (thisRecord.ArtworkLarge == null)
                            {
                                try
                                {
                                    thisRecord.ArtworkLarge =
                                        CloudinaryConnector.UploadRemoteImage(new ImageInfo {Link = artworkLink}, "iOS");
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
                        //if (!string.IsNullOrEmpty(recordInfo[10]))
                        //{
                        //    thisRecord.ArtworkSmall = GetOrUploadCloudinaryImage(recordInfo[10]);
                        //}

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

        private static AppScreenshot GetAppScreenshotFromLink(string link)
        {
            return (AppScreenshot) iOSDatabaseConnector.Execute(db =>
                {
                    var appScreenshot =
                        db.AppScreenshots.FirstOrDefault(
                            x => x.CloudinaryImage.OriginalLink.Equals(link, StringComparison.OrdinalIgnoreCase));
                    if (appScreenshot == null)
                    {
                        appScreenshot = new AppScreenshot
                            {
                                CloudinaryImage = CloudinaryConnector.UploadRemoteImage(new ImageInfo {Link = link})
                            };
                    }
                    return appScreenshot;
                });
        }

        public static void AddOrUpdateAppDetails(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                var lockedUntilUtc = messages.First().LockedUntilUtc;

                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);

                    int appId = Convert.ToInt32(recordInfo[1]);
                    var thisRecord = new iOSAppDetail
                    {
                        ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                        App = db.iOSApps.First(x => x.Id == appId),
                        LanguageCode = recordInfo[2],
                        Title = recordInfo[3],
                        Description = recordInfo[4],
                        ReleaseNotes = recordInfo[5],
                        CompanyUrl = recordInfo[6],
                        SupportUrl = recordInfo[7]
                    };

                    for (int j = 8; j < 12; j++)
                    {
                        var appScreenshot = GetAppScreenshotFromLink(recordInfo[j]);
                        thisRecord.AppScreenshots.Add(appScreenshot);
                    }

                    var databaseRecord = db.iOSAppDetails.FirstOrDefault(x => x.App.Id == thisRecord.App.Id && x.LanguageCode == thisRecord.LanguageCode);
                    if (databaseRecord == null)
                    {
                        db.iOSAppDetails.Add(thisRecord);
                    }
                    else
                    {
                        db.Entry(databaseRecord).CurrentValues.SetValues(thisRecord);

                        var removedScreenshots = databaseRecord.AppScreenshots.Where(s => thisRecord.AppScreenshots.All(i => i.Id != s.Id));
                        foreach (AppScreenshot screenshot in removedScreenshots)
                        {
                            if (screenshot.CloudinaryImage == null) continue;
                            CloudinaryConnector.DeleteImage(screenshot.CloudinaryImage);
                            db.CloudinaryImages.Remove(screenshot.CloudinaryImage);
                        }
                        databaseRecord.AppScreenshots = thisRecord.AppScreenshots;
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
                var lockedUntilUtc = messages.First().LockedUntilUtc;
                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);
                    int appId = Convert.ToInt32(recordInfo[1]);
                    int deviceTypeId = Convert.ToInt32(recordInfo[2]);
                    var databaseRecord = db.iOSAppDeviceTypes.FirstOrDefault(x => x.App.Id == appId && x.DeviceType.Id == deviceTypeId);
                    if (databaseRecord == null)
                    {
                        var newRecord = new iOSAppDeviceType
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = db.iOSApps.First(x => x.Id == appId),
                            DeviceType = db.iOSDeviceTypes.First(x => x.Id == deviceTypeId)
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
            var lockedUntilUtc = messages.First().LockedUntilUtc;
            iOSDatabaseConnector.Execute(db =>
            {
                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);
                    int artistId = Convert.ToInt32(recordInfo[1]);
                    int appId = Convert.ToInt32(recordInfo[2]);
                    var databaseRecord = db.iOSAppArtists.FirstOrDefault(x => x.App.Id == appId && x.Artist.Id == artistId);
                    if (databaseRecord == null)
                    {
                        var newRecord = new iOSAppArtist
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = db.iOSApps.First(x => x.Id == appId),
                            Artist = db.iOSArtists.First(x => x.Id == artistId)
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

        public static void AddOrUpdateAppGenres(IEnumerable<IList<string>> records, BrokeredMessage[] messages)
        {
            iOSDatabaseConnector.Execute(db =>
            {
                var lockedUntilUtc = messages.First().LockedUntilUtc;
                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);
                    int genreId = Convert.ToInt32(recordInfo[1]);
                    int appId = Convert.ToInt32(recordInfo[2]);

                    var databaseRecord = db.iOSAppGenres.FirstOrDefault(x => x.App.Id == appId && x.Genre.Id == genreId);
                    if (databaseRecord == null)
                    {
                        var newRecord = new iOSAppGenre
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = db.iOSApps.First(x => x.Id == appId),
                            Genre = db.iOSGenres.First(x => x.Id == genreId),
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
                var lockedUntilUtc = messages.First().LockedUntilUtc;
                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);
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
                var lockedUntilUtc = messages.First().LockedUntilUtc;
                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);
                    int appId = Convert.ToInt32(recordInfo[1]);
                    int storefrontId = Convert.ToInt32(recordInfo[4]);
                    var databaseRecord = db.iOSAppPrices.FirstOrDefault(x => x.App.Id == appId && x.Storefront.Id == storefrontId);
                    if (databaseRecord == null)
                    {
                        var newRecord = new iOSAppPrice
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = db.iOSApps.First(x => x.Id == appId),
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
                var lockedUntilUtc = messages.First().LockedUntilUtc;
                foreach (IList<string> recordInfo in records)
                {
                    lockedUntilUtc = RenewLocksIfNeeded(messages, lockedUntilUtc);
                    int storefrontId = Convert.ToInt32(recordInfo[1]);
                    int genreId = Convert.ToInt32(recordInfo[2]);
                    int appId = Convert.ToInt32(recordInfo[3]);
                    var databaseRecord = db.iOSAppPopularityPerGenres.FirstOrDefault(x => x.App.Id == appId && 
                                                        x.Storefront.Id == storefrontId && x.Genre.Id == genreId);
                    if (databaseRecord == null)
                    {
                        var newRecord = new iOSAppPopularityPerGenre
                        {
                            ExportDate = Util.EpochPlusMilliseconds(recordInfo[0]),
                            App = db.iOSApps.First(x => x.Id == appId),
                            Genre = db.iOSGenres.First(x => x.Id == genreId),
                            Storefront = db.iOSStorefronts.First(x => x.Id == storefrontId),
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

        private static DateTime RenewLocksIfNeeded(BrokeredMessage[] messages, DateTime lockedUntilUtc)
        {
            TimeSpan renewDelta = TimeSpan.FromSeconds(30);
            if (DateTime.UtcNow > lockedUntilUtc.Subtract(renewDelta))
            {
                foreach (var message in messages)
                {
                    message.RenewLock();
                }
                return messages.First().LockedUntilUtc;
            }
            return lockedUntilUtc;
        }
    }
}
