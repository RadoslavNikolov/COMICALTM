namespace Contests.App.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Data.UnitOfWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Models.BindingModels;
    using Models.ViewModels;
    using Toastr;

    public class HomeController : BaseAdminController
    {
        public HomeController(IContestsData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var categories = this.ContestsData.Categories.All()
                .Project()
                .To<CategoryViewModel>()
                .AsEnumerable();


            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TestBindingModel model)
        {
            if (model.Elements == null)
            {
                this.AddToastMessage("Info", "No categories to activate.", ToastType.Info);
            }
            else
            {
                var categories = this.ContestsData.Categories.All()
                .Where(c => model.Elements.Contains(c.Id) && c.IsActive == false);

                if (categories.Any())
                {
                    var message = new List<string>();

                    foreach (var category in categories)
                    {
                        category.IsActive = true;
                        message.Add(category.Name);
                    }

                    this.ContestsData.SaveChanges();

                    this.AddToastMessage("Success", "Categories " + String.Join(", ", message) + " activated.",
                        ToastType.Success);
                }
                else
                {
                    this.AddToastMessage("Info", "No categories to activate.", ToastType.Info);
                }
            }

            return this.RedirectToAction("Index", "Categories");
        }
    }
}