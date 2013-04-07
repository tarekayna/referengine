//using System.Globalization;
//using System.IO;
//using Microsoft.WindowsAzure.Storage;
//using System.Configuration;
//using ReferEngine.Common.Models;

//namespace ReferEngine.Web.DataAccess
//{
//    public class BlobStorageOperations
//    {
//        public static void UploadAppScreenshot(AppScreenshot appScreenshot, Stream fileStream)
//        {
//            string connectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
//            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

//            var blobClient = storageAccount.CreateCloudBlobClient();
//            var container = blobClient.GetContainerReference("app-screenshots");

//            var blockBlob = container.GetBlockBlobReference(appScreenshot.Id.ToString(CultureInfo.InvariantCulture));

//            blockBlob.UploadFromStream(fileStream);
//        }
//    }
//}