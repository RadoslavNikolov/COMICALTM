namespace Contests.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.UnitOfWork;
    using Infrastructure;
    using Models.ViewModels;
    using MvcPaging;

    public class NotificationsController : BaseController
    {
        public NotificationsController(IContestsData data)
            : base(data)
        {
        }

        [Authorize]
        // GET: Notifications
        public ActionResult Index(int? page)
        {
            int currentPageIndex = page ?? 1;

            var notifications = this.ContestsData.Notifications.All()
                .Where(n => n.UserId == this.UserProfile.Id)
                .OrderByDescending(n => n.Date);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            this.ContestsData.SaveChanges();
            this.ViewBag.notifications = 0;

            var result = notifications
                .Project()
                .To<NotificationViewModel>()
                .ToPagedList(currentPageIndex, AppConfig.NotificationsPageSize);

            return View(result);
        }
    }
}