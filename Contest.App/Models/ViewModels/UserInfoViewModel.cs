namespace Contests.App.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Contests.Models;

    public class UserInfoViewModel
    {
        public string Id { get; set; }

        [Display(Name = "User name: ")]
        public string UserName { get; set; }

        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Display(Name = "Phone number: ")]
        public string PhoneNumber { get; set; }

        public ICollection<File> Files { get; set; }
    }
}