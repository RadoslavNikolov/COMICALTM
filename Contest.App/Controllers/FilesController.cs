using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contests.App.Controllers
{

    using System.Web;
    using System.Web.Mvc;
    using Contest.App;
    using Contests.Models;
    using Data.UnitOfWork;
    using Helpers;
    using Infrastructure;
    using Microsoft.AspNet.Identity.Owin;
    using Models.BindingModels;
    using Models.ViewModels;

    public class FilesController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private UserManager _userManager;

        public FilesController(IContestsData data, UserManager userManager, ApplicationSignInManager signInManager)
            : this(data)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public FilesController(IContestsData data)
            : base(data)
        {
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            private set { _userManager = value; }
        }


        public ActionResult Index(int id)
        {
            var fileToRetrieve = this.ContestsData.Files.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(PhotoBindingModel model)
        {

            if (ModelState.IsValid)
            {


                if (model.Upload != null)
                {
                    string path = Dropbox.Upload(model.Upload.FileName, model.Upload.InputStream);
                    var newPhoto = new Photo
                    {
                        Path = path,
                        CreatedOn = DateTime.Now,
                        Owner = this.UserProfile
                    };

                    this.UserProfile.Photos.Add(newPhoto);
                    this.ContestsData.SaveChanges();

                    this.TempData["Success"] = new[] { "Edit successfull" };
                }

                return this.View();

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid model");
        }


        [HttpGet]
        [Authorize]
        public ActionResult List()
        {
            var photos = this.UserProfile.Photos;
            List<PhotoViewModel> photoModels = new List<PhotoViewModel>();

            foreach (var photo in photos)
            {
                photoModels.Add(new PhotoViewModel()
                {
                    Url = Dropbox.Download(photo.Path),
                    CreatedOn = photo.CreatedOn
                });
            }

            //this.ViewBag.Photos = photoUrls.OrderByDescending(p => p.CreatedOn)
            //    .Select(u => Dropbox.Download(u.Url));

            var result = photoModels.OrderByDescending(m => m.CreatedOn);
            return this.View(result);
        }
    }
}
