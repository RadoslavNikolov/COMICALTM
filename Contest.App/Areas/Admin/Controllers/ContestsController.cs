namespace Contests.App.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Data.UnitOfWork;

    public class ContestsController : BaseAdminController
    {
        public ContestsController(IContestsData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}