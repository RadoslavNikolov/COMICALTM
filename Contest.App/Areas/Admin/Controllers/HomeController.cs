using System.Web.Mvc;
using Contests.Data.UnitOfWork;

namespace Contests.App.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
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