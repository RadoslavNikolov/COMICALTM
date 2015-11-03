namespace Contests.App.Controllers
{
    using System.Web.Mvc;
    using Data.UnitOfWork;

    public class ErrorController : BaseController
    {
        public ErrorController(IContestsData data) 
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View("Error");
        }
    }
}