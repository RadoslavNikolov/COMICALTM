using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contests.App.Controllers
{
    using System.Web.Routing;
    using Contests.Data.Interfaces;
    using Contests.Data.UnitOfWork;
    using Contests.Models;

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