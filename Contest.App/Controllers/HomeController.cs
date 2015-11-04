namespace Contests.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.UnitOfWork;
    using Infrastructure;
    using Models.ViewModels;
    using PagedList;
    using Toastr;

    public class HomeController : BaseController
    {
        public HomeController(IContestsData data)
            : base(data)
        {
        }


        public ActionResult Index(int? page)
        {
            var contests = this.ContestsData.Contests.All()
               .Where(c => c.IsActive)
               .OrderByDescending(c => c.CreatedOn)
               .Project()
               .To<ContestViewModel>()
               .ToPagedList(pageNumber: page ?? 1, pageSize: AppConfig.AdminPanelPageSize);

            if (!contests.Any())
            {
                this.AddToastMessage("Info", "No contests found", ToastType.Info);
            }

            return View(contests);
        }
    }
}