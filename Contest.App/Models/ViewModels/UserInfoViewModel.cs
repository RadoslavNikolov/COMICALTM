namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using Contests.Models;
    using Helpers;

    public class UserInfoViewModel
    {
        public string Id { get; set; }

        [Display(Name = "User name: ")]
        public string UserName { get; set; }

        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Display(Name = "Phone number: ")]
        public string PhoneNumber { get; set; }

        public string ProfileImageUrl { get; set; }

        public string ThumbProfileUrl { get; set; }


        public static Expression<Func<User, UserInfoViewModel>> Create
        {
            get
            {
                return u => new UserInfoViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                };
            }
        }
    }
}