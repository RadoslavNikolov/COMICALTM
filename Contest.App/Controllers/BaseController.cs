namespace Contests.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Contests.Models;
    using Data.UnitOfWork;

    public class BaseController : Controller
    {
        public BaseController(IContestsData data)
        {
            this.ContestsData = data;
        }

        public BaseController(IContestsData data, User user)
            : this(data)
        {
            this.UserProfile = user;
        }

        public IContestsData ContestsData { get; private set; }

        public User UserProfile { get; set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                var user = this.ContestsData.Users.All().FirstOrDefault(u => u.UserName == username);
                this.UserProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}