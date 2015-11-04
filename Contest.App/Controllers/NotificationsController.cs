namespace Contests.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Contests.Models;
    using Data.UnitOfWork;
    using Models.ViewModels;
    using RestSharp.Validation;

    public class NotificationsController : BaseController
    {
        public NotificationsController(IContestsData data)
            : base(data)
        {
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddNotification(string userId, string message)
        {
            var targetUser = this.ContestsData.Users.Find(userId);

            if (targetUser == null)
            {
                return this.HttpNotFound();
            }

            var notification = new Notification
            {
                Message = message,
                IsRead = false,
                Date = DateTime.Now
            };

            targetUser.Notifications.Add(notification);
            this.ContestsData.SaveChanges();

            return this.Content("true");
        }

        [Authorize]
        // GET: Notifications
        public ActionResult Index()
        {
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
                .To<NotificationViewModel>();

            return View(result);
        }
    }
}