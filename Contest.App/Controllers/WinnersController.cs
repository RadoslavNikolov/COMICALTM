using System.Linq;
using System.Web.Mvc;

namespace Contests.App.Controllers
{
    using Areas.Admin.Models.ViewModels;
    using AutoMapper.QueryableExtensions;
    using Data.UnitOfWork;
    using Infrastructure;
    using Models.ViewModels;
    using PagedList;
    using Toastr;

    public class WinnersController : BaseController
    {
        public WinnersController(IContestsData data) : base(data)
        {
        }


        public ActionResult Index(int? page)
        {
            var winningContests = this.ContestsData.Contests.All()
                .Where(c => c.IsFinalized)
                .OrderByDescending(c => c.FinishedOn)
                .Project()
                .To<WinningContestViewModel>()
                .ToPagedList(pageNumber: page ?? 1, pageSize: AppConfig.AdminPanelPageSize);

            return this.View(winningContests);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            var contest = this.ContestsData.Contests.All()
                .Where(c => c.Id == id && c.IsFinalized)
                .Project()
                .To<WinningContestViewModel>();

            var test = contest.ToList();

            if (contest == null)
            {
                this.AddToastMessage("Info", "No such contest found", ToastType.Info);
                return this.RedirectToAction("Index", "Home", routeValues: new { area = "" });
            }

            return this.View(contest);

        }
    }
}