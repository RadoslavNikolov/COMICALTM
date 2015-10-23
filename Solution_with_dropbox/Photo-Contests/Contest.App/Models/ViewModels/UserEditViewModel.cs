namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BindingModels;
    using Contests.Models;

    public class UserEditViewModel : UserEditBindingModel
    {
        public static Expression<Func<User, UserEditViewModel>> Create
        {
            get
            {
                return u => new UserEditViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImage = u.Files.FirstOrDefault() != null ? u.Files.FirstOrDefault().FileId : -1
                };
            }
        }
    }
}