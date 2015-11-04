namespace Contests.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contests.Models;
    using Data.UnitOfWork;
    using Infrastructure;
    using Infrastructure.UserIdProvider;

    public class BaseController : Controller
    {


        public BaseController(IContestsData data)
            : this(data, new AspNetUserIdProvider())
        {
            this.ContestsData = data;
        }

        public BaseController(IContestsData data, IUserIdProvider userIdProvider)
        {
            this.ContestsData = data;
            this.UserIdProvider = userIdProvider;
            this.UserProfile = new User();
        }

        public IContestsData ContestsData { get; private set; }

        public User UserProfile { get; set; }

        protected IUserIdProvider UserIdProvider { get; set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                var user = this.ContestsData.Users.All().FirstOrDefault(u => u.UserName == username);
                this.UserProfile = user;
                this.ViewBag.notifications = this.UserProfile.Notifications.Count(n => n.IsRead == false);
            }

            return base.BeginExecute(requestContext, callback, state);
        }

        protected bool IsAdmin()
        {
            return this.User.IsInRole(AppConfig.AdminRole);
        }

        protected ICollection<User> GetUsers(ICollection<string> usersId)
        {
            ICollection<User> users = new HashSet<User>();

            if (usersId != null)
            {
                foreach (string id in usersId)
                {
                    User wantedUser = this.ContestsData.Users.Find(id);
                    if (wantedUser == null)
                    {
                        throw new NullReferenceException();
                    }

                    users.Add(wantedUser);
                }
            }

            return users;
        }
    }
}