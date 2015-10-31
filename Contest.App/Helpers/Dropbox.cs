namespace Contests.App.Helpers
{
    using System;
    using System.IO;
    using System.Web.Http.Results;
    using Data;
    using DropNet;
    using Infrastructure;
    using Microsoft.Ajax.Utilities;
    using RestSharp;

    public static class Dropbox
    {
        private static DropNetClient client;

        static Dropbox()
        {
            client = new DropNetClient(AppKeys.DropboxApiKey, AppKeys.DropboxApiSecret, AppKeys.DropboxAccessTDropboxAccessToken);
        }


        internal static string Upload(string fileName, Stream fileStream)
        {
            var random = new Random();
            string fullFileName = "" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + random.Next(99) + "_" +fileName;
            client.UploadFile("/" + AppKeys.DropboxFolder + "/", fullFileName, fileStream);
            return fullFileName;
        }

        internal static string Upload(string fileName, Stream fileStream, string subFolder)
        {
            var random = new Random();
            string fullFileName = "" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + random.Next(99) + "_" + fileName;
            client.UploadFile("/" + AppKeys.DropboxFolder + "/" + subFolder + "/", fullFileName, fileStream);
            return fullFileName;
        }


        internal static string Download(string path)
        {
            return client.GetMedia("/" + AppKeys.DropboxFolder + "/" + path).Url;
        }

        internal static string Download(string path, string subFolder)
        {
            string fullPath = "/" + AppKeys.DropboxFolder;

            if (subFolder != null)
            {
                fullPath += "/" + subFolder;
            }

            fullPath += "/" + path;

            return client.GetMedia(fullPath).Url;
        }


        internal static void DeleteImages(string photoPath, string thumbnailPath)
        {
            if (photoPath != null)
            {
                Dropbox.Delete(photoPath);
            }

            if (thumbnailPath != null)
            {
                Dropbox.DeleteThumbnail(thumbnailPath);
            }
        }

        internal static void Delete(string path)
        {
            client.DeleteAsync("/" + AppKeys.DropboxFolder + "/" + path, response =>
            {
                string test = "test";
            }, exception =>
            {
                throw new NotImplementedException();
            });
        }

        internal static void DeleteThumbnail(string path)
        {
            client.DeleteAsync("/" + AppKeys.DropboxFolder + "/Thumbnails/" + path, response =>
            {
                string test = "test";
            }, exception =>
            {
                throw new NotImplementedException();
            });
        }

    }
}