namespace Contests.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.UnitOfWork;
    using Models.ViewModels;
    using Toastr;

    public class HomeController : BaseController
    {
        public HomeController(IContestsData data)
            : base(data)
        {
        }


        public ActionResult Index()
        {
            var contests = this.ContestsData.Contests.All()
               .Where(c => c.IsActive)
               .OrderByDescending(c => c.CreatedOn)
               .Project()
               .To<ContestViewModel>();

            if (!contests.Any())
            {
                this.AddToastMessage("Info", "No contests found", ToastType.Info);
            }

            return View(contests);
        }
    }
}