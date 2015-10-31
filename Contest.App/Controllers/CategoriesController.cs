using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contests.App.Controllers
{
    using AutoMapper.QueryableExtensions;
    using Data.UnitOfWork;
    using Models.ViewModels;
    using Toastr;

    public class CategoriesController : BaseController
    {
        public CategoriesController(IContestsData data) : base(data)
        {
        }


        public ActionResult Index(int id)
        {
            var contests = this.ContestsData.Contests.All()
                .Where(c => c.CategoryId == id && c.IsActive)
                .OrderByDescending(c => c.CreatedOn)
                .Project()
                .To<ContestViewModel>();

            if (!contests.Any())
            {
                this.AddToastMessage("Info", "No contest in " + this.ContestsData.Categories.Find(id).Name + " category", ToastType.Info);
                return this.RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(contests);
        }

        [AllowAnonymous]
        public ActionResult GetAllCategories()
        {
            var result = this.ContestsData.Categories.All()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Project()
                .To<CategoryViewModel>();

            return this.PartialView("Partial/_Categories", result);
        }
    }
}