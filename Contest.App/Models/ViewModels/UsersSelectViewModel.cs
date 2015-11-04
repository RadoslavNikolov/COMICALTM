namespace Contests.App.Models.ViewModels
{
    using System.Linq;
    using System.Web.Mvc;

    public class UsersSelectViewModel
    {
        public string IdAttribute { get; set; }

        public string NameAttribute { get; set; }

        public IQueryable<SelectListItem> Items { get; set; }
    }
}