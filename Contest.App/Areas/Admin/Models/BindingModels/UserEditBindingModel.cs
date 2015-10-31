namespace Contests.App.Areas.Admin.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Contests.Models;
    using Infrastructure.Mapping;

    public class UserEditBindingModel : IMapFrom<User>
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
    }
}