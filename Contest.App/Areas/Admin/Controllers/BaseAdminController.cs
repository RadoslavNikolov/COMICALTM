namespace Contests.App.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using App.Controllers;
    using Data.UnitOfWork;

    [Authorize(Roles = "Admin")]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(IContestsData data)
            : base(data)
        {
        }
    }
}