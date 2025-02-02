﻿using System.IO;
using System.Net;
using AppSmarts.Common.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageInfo = AppSmarts.Common.Models.ImageInfo;

namespace AppSmarts.Common.Data
{
    public class CloudinaryConnector
    {
        private static Account _cloudinaryAccount;
        private static Account CloudinaryAccount
        {
            get
            {
                return _cloudinaryAccount ??
                       (_cloudinaryAccount = new Account("referengine", "945283732626525", "HAobnQaoWPS2ECMTXP2iWO_84I8"));
            }
        }

        private static Cloudinary _cloudinary;
        private static Cloudinary Cloudinary
        {
            get { return _cloudinary ?? (_cloudinary = new Cloudinary(CloudinaryAccount)); }
        }

        private static string _baseUrl = "https://cloudinary-a.akamaihd.net/referengine/image/upload/";

        public static void DeleteImage(CloudinaryImage cloudinaryImage)
        {
            Cloudinary.DeleteResources(cloudinaryImage.Id);
        }

        public static CloudinaryImage UploadRemoteImage(ImageInfo imageInfo, string tags = null)
        {
            string t = string.IsNullOrEmpty(tags) ? Utilities.Util.CurrentServiceConfigurationString : tags + Utilities.Util.CurrentServiceConfigurationString;
            ImageUploadParams uploadParams = new ImageUploadParams
                                                 {
                                                     File = new FileDescription(imageInfo.Link),
                                                     UseFilename = false,
                                                     Tags = t
                                                 };
            ImageUploadResult uploadResult = Cloudinary.Upload(uploadParams);
            if (uploadResult.StatusCode == HttpStatusCode.NotFound || string.IsNullOrEmpty(uploadResult.PublicId)) return null;
            CloudinaryImage cloudinaryImage = new CloudinaryImage(uploadResult.PublicId, uploadResult.Format,
                                                                  imageInfo.Link) {Description = imageInfo.Description};
            return cloudinaryImage;
        }

        public static CloudinaryImage UploadImage(Stream fileStream, string fileName, string description = null)
        {
            FileDescription fileDescription = new FileDescription(fileName, fileStream);
            ImageUploadParams uploadParams = new ImageUploadParams();
            uploadParams.File = fileDescription;
            ImageUploadResult uploadResult = Cloudinary.Upload(uploadParams);
            CloudinaryImage cloudinaryImage = new CloudinaryImage(uploadResult.PublicId, uploadResult.Format) { Description = description };
            return cloudinaryImage;
        }

        public static string GetLink(CloudinaryImage cloudinaryImage, string transformation = null)
        {
            string link = _baseUrl;
            if (!string.IsNullOrEmpty(transformation))
            {
                link += transformation + "/";
            }
            link += cloudinaryImage.Id + "." + cloudinaryImage.Format;
            return link;
        }
    }
}
