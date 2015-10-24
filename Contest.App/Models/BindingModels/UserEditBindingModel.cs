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
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"^[\dA-Za-z_]{2,30}$",
            ErrorMessage = "The {0} must be between 2 and 30 characters long and contains letters, numbers or _")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [ValidateImage(ErrorMessage = "Please select an image smaller than 4MB")]
        public HttpPostedFileBase Upload { get; set; }

        public int? ProfileImageId { get; set; }

        public Photo ProfileImage { get; set; }

        public string ProfileImageUrl { get; set; }

        public string ThumbsProfileImageUrl { get; set; }
    }
}