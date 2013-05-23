using System;
using System.Collections.Generic;
using ReferEngine.Common.Models;
using ReferEngine.Common.Models.iOS;
using System.Linq;
using ReferEngine.Common.ViewModels.AppStore.iOS;

namespace ReferEngine.Common.Data.iOS
{
    public static class iOSDataReader
    {
        public static iOSDataImport GetLastDataImport()
        {
            return iOSDatabaseReader.GetLastDataImport();
        }

        public static iOSApp GetApp(int id)
        {
            var result = iOSCacheOperations.App.Get(id);
            if (result == null)
            {
                result = iOSDatabaseReader.GetApp(id);
                if (result != null)
                {
                    iOSCacheOperations.App.Add(result);
                }
            }
            return result;
        }

        public static iOSAppViewModel GetAppViewModelByName(string name)
        {
            iOSAppViewModel viewModel = iOSCacheOperations.AppViewModelByName.Get(name);
            if (viewModel == null)
            {
                viewModel = iOSDatabaseReader.GetAppViewModelByName(name);
                if (viewModel != null)
                {
                    iOSCacheOperations.AppViewModelByName.Add(viewModel);
                }
            }
            return viewModel;
        }

        public static IList<iOSGenre> GetAppStoreGenres()
        {
            var result = iOSCacheOperations.AppStoreGenres.Get();
            if (result == null)
            {
                result = iOSDatabaseReader.GetAppStoreGenres();
                if (result != null)
                {
                    iOSCacheOperations.AppStoreGenres.Add(result);
                }
            }
            return result;
        }
    }
}
