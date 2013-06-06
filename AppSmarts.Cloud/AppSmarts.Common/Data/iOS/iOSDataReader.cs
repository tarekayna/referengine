using System.Collections.Generic;
using AppSmarts.Common.Models.iOS;
using AppSmarts.Common.ViewModels.AppStore.iOS;

namespace AppSmarts.Common.Data.iOS
{
    public static class iOSDataReader
    {
        public static iOSDataImportStep GetDataImportStep(ImportType importType, string dateString, ImportStepName stepName)
        {
            return iOSDatabaseReader.GetDataImportStep(importType, dateString, stepName);
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
