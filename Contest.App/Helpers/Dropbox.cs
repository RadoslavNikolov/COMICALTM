﻿namespace Contests.App.Helpers
{
    using System;
    using System.IO;
    using Data;
    using DropNet;
    using Infrastructure;

    public static class Dropbox
    {
        private static DropNetClient client;

        static Dropbox()
        {
            client = new DropNetClient(AppKeys.DropboxApiKey, AppKeys.DropboxApiSecret, AppKeys.DropboxAccessTDropboxAccessToken);
        }


        public static string Upload(string fileName, Stream fileStream)
        {
            var random = new Random();
            string fullFileName = "" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + random.Next(99) + "_" +fileName;
            client.UploadFile("/" + AppKeys.DropboxFolder + "/", fullFileName, fileStream);
            return fullFileName;
        }


        public static string Download(string path)
        {
            return client.GetMedia("/" + AppKeys.DropboxFolder + "/" + path).Url;
        }

    }
}