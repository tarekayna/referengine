using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using AppSmarts.Common.Models;
using AppSmarts.Common.Models.iOS;
using AppSmarts.Common.ViewModels.AppStore.iOS;

namespace AppSmarts.Common.Data.iOS
{
    internal static class iOSDatabaseReader
    {
        internal static iOSArtistType GetArtistType(int id)
        {
            return (iOSArtistType)iOSDatabaseConnector.Execute(db => db.iOSArtistTypes.First(x => x.Id == id));
        }

        internal static iOSApp GetApp(int id)
        {
            return (iOSApp)iOSDatabaseConnector.Execute(db => db.iOSApps.First(x => x.Id == id));
        }

        internal static iOSDeviceType GetDeviceType(int id)
        {
            return (iOSDeviceType)iOSDatabaseConnector.Execute(db => db.iOSDeviceTypes.First(x => x.Id == id));
        }

        internal static iOSStorefront GetStorefront(int id)
        {
            return (iOSStorefront)iOSDatabaseConnector.Execute(db => db.iOSStorefronts.First(x => x.Id == id));
        }

        internal static iOSMediaType GetMediaType(int id)
        {
            return (iOSMediaType)iOSDatabaseConnector.Execute(db => db.iOSMediaTypes.First(x => x.Id == id));
        }

        internal static iOSAppDeviceType GetAppDeviceType(int appId, int deviceTypeId)
        {
            return (iOSAppDeviceType)iOSDatabaseConnector.Execute(db => db.iOSAppDeviceTypes.First(x => x.App.Id == appId && x.DeviceType.Id == deviceTypeId));
        }

        internal static iOSAppArtist GetAppArtist(int artistId, int appId)
        {
            return (iOSAppArtist)iOSDatabaseConnector.Execute(db => db.iOSAppArtists.First(x => x.App.Id == appId && x.Artist.Id == artistId));
        }

        internal static iOSAppGenre GetAppGenre(int genreId, int appId)
        {
            return (iOSAppGenre)iOSDatabaseConnector.Execute(db => db.iOSAppGenres.First(x => x.App.Id == appId && x.Genre.Id == genreId));
        }

        internal static iOSAppPrice GetAppPrice(int appId, int storefrontId)
        {
            return (iOSAppPrice)iOSDatabaseConnector.Execute(db => db.iOSAppPrices.First(x => x.Id == appId && x.Storefront.Id == storefrontId));
        }

        internal static iOSAppPopularityPerGenre GetAppPopularityPerGenre(int appId, int storefrontId, int genreId)
        {
            return (iOSAppPopularityPerGenre)iOSDatabaseConnector.Execute(db => db.iOSAppPopularityPerGenres.First(x => x.Id == appId &&
                                x.Storefront.Id == storefrontId && x.Genre.Id == genreId));
        }

        internal static iOSArtist GetArtist(int artistId)
        {
            return (iOSArtist)iOSDatabaseConnector.Execute(db => db.iOSArtists.First(x => x.Id == artistId));
        }

        internal static iOSGenre GetGenre(int genreId)
        {
            return (iOSGenre)iOSDatabaseConnector.Execute(db => db.iOSGenres.First(x => x.Id == genreId));
        }

        internal static AppScreenshot GetAppScreenshot(string originalLink)
        {
            return (AppScreenshot)iOSDatabaseConnector.Execute(db => db.AppScreenshots.FirstOrDefault(
                x => x.CloudinaryImage.OriginalLink.Equals(originalLink, StringComparison.OrdinalIgnoreCase)));
        }

        internal static iOSDataImportStep GetDataImportStep(ImportType importType, string dateString, ImportStepName stepName)
        {
            return (iOSDataImportStep)iOSDatabaseConnector.Execute(db =>
                                        db.DataImportSteps.FirstOrDefault(x => x.DateString == dateString &&
                                                                               x.ImportType == importType &&
                                                                               x.Name == stepName));
        }

        internal static iOSAppViewModel GetAppViewModelByName(string appName)
        {
            return (iOSAppViewModel)iOSDatabaseConnector.Execute(db =>
            {
                string name = HttpUtility.HtmlEncode(appName);
                iOSAppViewModel viewModel = new iOSAppViewModel();
                viewModel.App = db.iOSApps.First(x => x.Title.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (viewModel.App != null)
                {
                    viewModel.AppDetail = db.iOSAppDetails.First(x => x.App.Id == viewModel.App.Id && x.LanguageCode == "EN");
                }
                return viewModel;
            });
        }

        internal static IList<iOSGenre> GetAppStoreGenres()
        {
            return (IList<iOSGenre>)iOSDatabaseConnector.Execute(db => db.iOSGenres.Where(x => x.ParentGenre.Id == 36).ToList());
        }
    }
}
