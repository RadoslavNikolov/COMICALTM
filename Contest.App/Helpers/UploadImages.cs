namespace Contests.App.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Hosting;
    using ImageResizer;
    using Infrastructure;

    public static class UploadImages
    {
        internal static List<string> UploadImage(HttpPostedFileBase upload, bool isProfile)
        {
            var basePath = HostingEnvironment.ApplicationPhysicalPath;
            var path = new List<string>();
            var commonResizeSettings = new ResizeSettings(AppConfig.ResizePhotoSettings);
            var thumbsResizeSettings = new ResizeSettings(AppConfig.ResizePhotoThumbSettings);

            if (isProfile)
            {
                commonResizeSettings = new ResizeSettings(AppConfig.RezieProfilePhotoSettings);
            }

            using (Stream newFile = System.IO.File.Create(basePath + "\\content\\temp.jpg"))
            {
                ImageJob i = new ImageJob();
                i.ResetSourceStream = true;
                i = new ImageJob(upload.InputStream, newFile, commonResizeSettings);
                i.CreateParentDirectory = false; //Auto-create the uploads directory.
                i.Build();
            }

            using (FileStream file = new FileStream(basePath + "\\content\\temp.jpg", FileMode.Open))
            {
                path.Add(Dropbox.Upload(upload.FileName, file));
            }

            ImageBuilder.Current.Build(basePath + "\\content\\temp.jpg", basePath + "\\content\\temp.jpg", thumbsResizeSettings);

            using (FileStream file = new FileStream(basePath + "\\content\\temp.jpg", FileMode.Open))
            {
                path.Add(Dropbox.Upload(upload.FileName, file, "Thumbnails"));

            }

            return path;
        }
    }
}