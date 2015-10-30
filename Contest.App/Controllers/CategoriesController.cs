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

    public class CategoriesController : BaseController
    {
        public CategoriesController(IContestsData data) : base(data)
        {
        }


        [HttpGet]
        public ActionResult Index(int? categoryId)
        {
            var result = this.ContestsData.Categories.All()
                .Where(c => c.Id == categoryId)
                .Project()
                .To<CategoryViewModel>().FirstOrDefault();

            return View(result);
        }


        [AllowAnonymous]
        public ActionResult GetAllCategories()
        {
            var result = this.ContestsData.Categories.All()
                .OrderBy(c => c.Name)
                .Project()
                .To<CategoryViewModel>();

            return this.PartialView("Partial/_Categories", result);
        }
    }
}