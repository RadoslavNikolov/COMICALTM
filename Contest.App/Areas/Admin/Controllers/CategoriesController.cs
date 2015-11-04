namespace Contests.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Models.ViewModels;
    using Contests.Models;
    using Data.UnitOfWork;
    using Infrastructure;
    using Models.BindingModels;
    using MvcPaging;
    using Toastr;

    public class CategoriesController : BaseAdminController
    {
        public CategoriesController(IContestsData data)
            : base(data)
        {
        }

        // GET: Admin/Categories
        public ActionResult Index(string category_name, int? page)
        {
            this.TempData["category_name"] = category_name;
            int currentPageIndex = page ?? 1;

            var categoriesQuery = this.ContestsData.Categories.All();

            if (!string.IsNullOrWhiteSpace(category_name))
            {
                categoriesQuery = categoriesQuery.Where(c => c.Name.Contains(category_name));

            }

            var categories = categoriesQuery
                    .OrderBy(c => c.Name)
                    .Project()
                    .To<CategoryViewModel>()
                    .ToPagedList(currentPageIndex, AppConfig.AdminPanelPageSize);

            return View(categories);
        }

        // GET: Admin/Categories/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryBindingModel model)
        {
            if (model != null || this.ModelState.IsValid)
            {
                if (this.ContestsData.Categories.All().All(c => c.Name != model.Name))
                {
                    var category = new Category
                    {
                        Name = model.Name,
                        IsActive = true
                    };

                    this.ContestsData.Categories.Add(category);
                    this.ContestsData.SaveChanges();

                    this.AddToastMessage("Success", "Category created.", ToastType.Success);

                    return this.RedirectToAction("Index");
                }

                this.ModelState.AddModelError("", @"Category already exists!");
            }

            return View(model);
        }

        // GET: Admin/Categories/Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = this.ContestsData.Categories.All()
                .Where(c => c.Id == id)
                .Project()
                .To<CategoryBindingModel>()
                .FirstOrDefault();

            if (category == null)
            {
                this.AddToastMessage("Error", "Non existing category!", ToastType.Error);

                return this.RedirectToAction("Index");
            }

            return View(category);
        }

        // POST: Admin/Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryBindingModel model)
        {
            if (model != null || this.ModelState.IsValid)
            {
                var category = this.ContestsData.Categories.All()
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    this.AddToastMessage("Error", "Non existing category!", ToastType.Error);

                    return this.RedirectToAction("Index");
                }

                if (!this.ContestsData.Categories.All().Any(c => c.Name == model.Name && c.Id != id))
                {
                    category.Name = model.Name;
                    this.ContestsData.SaveChanges();

                    this.AddToastMessage("Success", "Category edited.", ToastType.Success);

                    return this.RedirectToAction("Index");
                }

                this.ModelState.AddModelError("", @"Category already exists!");
            }

            return View(model);
        }

        // GET: Admin/Categories/Activate
        [HttpGet]
        public ActionResult Activate(int id)
        {
            var category = this.ContestsData.Categories.All()
                .Where(c => c.Id == id)
                .Project()
                .To<CategoryBindingModel>()
                .FirstOrDefault();

            if (category == null)
            {
                this.AddToastMessage("Error", "Non existing category!", ToastType.Error);

                return this.RedirectToAction("Index");
            }

            return View(category);
        }

        // POST: Admin/Categories/Activate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id, CategoryBindingModel model)
        {
            var category = this.ContestsData.Categories.All()
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                this.AddToastMessage("Error", "Non existing category!", ToastType.Error);

                return this.RedirectToAction("Index");
            }

            category.IsActive = true;
            this.ContestsData.SaveChanges();

            this.AddToastMessage("Success", "Category activated.", ToastType.Success);

            return this.RedirectToAction("Index");
        }

        // GET: Admin/Categories/Deactivate
        [HttpGet]
        public ActionResult Deactivate(int id)
        {
            var category = this.ContestsData.Categories.All()
                .Where(c => c.Id == id)
                .Project()
                .To<CategoryBindingModel>()
                .FirstOrDefault();

            if (category == null)
            {
                this.AddToastMessage("Error", "Non existing category!", ToastType.Error);

                return this.RedirectToAction("Index");
            }

            return View(category);
        }

        // POST: Admin/Categories/Deactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, CategoryBindingModel model)
        {
            var category = this.ContestsData.Categories.All()
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                this.AddToastMessage("Error", "Non existing category!", ToastType.Error);

                return this.RedirectToAction("Index");
            }

            category.IsActive = false;
            this.ContestsData.SaveChanges();

            this.AddToastMessage("Success", "Category deactivated.", ToastType.Success);

            return this.RedirectToAction("Index");
        }
    }
}