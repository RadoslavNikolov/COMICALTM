using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contests.App.Controllers
{
    using Contests.Data.UnitOfWork;

    public class HomeController : BaseController
    {
        public HomeController(IContestsData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

    
    }
}