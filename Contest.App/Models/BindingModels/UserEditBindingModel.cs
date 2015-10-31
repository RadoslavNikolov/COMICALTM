namespace Contests.App.Models.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using Contests.Models;
    using Validators;

    public class UserEditBindingModel
    {
        [Required]
        //[RegularExpression(@"^[\dA-Za-z_]{2,30}$",
        //    ErrorMessage = "The {0} must be between 2 and 30 characters long and contains letters, numbers or _")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        //[RegularExpression(@"^[\dA-Za-z_]{2,30}$",
        //    ErrorMessage = "The {0} must be between 2 and 30 characters long and contains letters, numbers or _")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [ValidateImage(ErrorMessage = "Please select an image smaller than 4MB")]
        public HttpPostedFileBase Upload { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public string ProfilePhotoPath { get; set; }

        public string ThumbnailPath { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string ThumbnailUrl { get; set; }


        public static UserEditBindingModel CreateFromUser(User user)
        {
            var newUser = new UserEditBindingModel()
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Upload = null,
                RegisteredOn = user.RegisteredOn,
                ProfilePhotoPath = user.ProfilePhotoPath,
                ThumbnailPath = user.ThumbnailPath,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                ThumbnailUrl = user.ThumbnailUrl
            };

            return newUser;
        }       
    }
}