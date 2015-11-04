namespace Contests.App.Controllers
{
    using System.Web.Mvc;
    using Data.UnitOfWork;

    public class NotificationsController : BaseController
    {
        public NotificationsController(IContestsData data)
            : base(data)
        {
        }

        // GET: Notifications
        public ActionResult Index()
        {
            return View();
        }
    }
}