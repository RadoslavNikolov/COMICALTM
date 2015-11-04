namespace Contests.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.UI.WebControls;
    using Contests.Models;
    using Infrastructure.Mapping;
    using Microsoft.AspNet.Identity;
    using Validators;

    public class PhotoBindingModel : IMapFrom<Photo>
    {
        [Required]
        public string UserId { get; set; }


        [ValidateImage(ErrorMessage = "Please select an image smaller than 4MB")]
        public HttpPostedFileBase Upload { get; set; }

        public int ContetsId { get; set; }

        public static PhotoBindingModel CreateFrom(Contest contest)
        {
            var newPhoto = new PhotoBindingModel
            {
               UserId = HttpContext.Current.User.Identity.GetUserId(),
               ContetsId = contest.Id
            };

            return newPhoto;
        }
    }
}