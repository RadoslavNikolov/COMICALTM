namespace Contests.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Validators;

    public class PhotoBindingModel
    {
        [Required]
        public string UserId { get; set; }


        [ValidateImage(ErrorMessage = "Please select an image smaller than 4MB")]
        public HttpPostedFileBase Upload { get; set; } 
    }
}