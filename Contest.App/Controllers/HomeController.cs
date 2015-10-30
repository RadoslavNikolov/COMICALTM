﻿namespace Contests.App.Controllers
{
    using System.Web.Mvc;
    using Data.UnitOfWork;

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