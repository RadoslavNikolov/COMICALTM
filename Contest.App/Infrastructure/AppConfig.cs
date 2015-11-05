namespace Contests.App.Infrastructure
{
    using System.Web.Hosting;

    public static class AppConfig
    {
        public static string DirectoryName = "PhotoContests";
        public const int AdminPanelPageSize = 5;
        public const int NotificationsPageSize = 3;

        // Roles
        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        //Images
        public const string ResizePhotoSettings = "width=800;height=800;format=jpg;mode=max";
        public const string ResizePhotoThumbSettings = "width=200;height=200;format=jpg;mode=max";
        public const string RezieProfilePhotoSettings = "width=400;height=400;format=jpg;mode=max";

    }
}