namespace Contests.App.Areas.Admin.Models.ViewModels
{
    using System.Collections.Generic;

    public class AdminsAndUsersViewModel
    {
        public IEnumerable<UserViewModel> Admins { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}