using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contests.App.Controllers
{
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Contest.App;
    using Contests.Models;
    using Data.UnitOfWork;
    using Helpers;
    using ImageResizer;
    using Infrastructure;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.SqlServer.Server;
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


        [HttpGet]
        public ActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult Upload(PhotoBindingModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.Upload != null)
                {
                    var paths = Helpers.UploadImages.UploadImage(model.Upload, false);

                    var newPhoto = new Photo
                    {
                        CreatedOn = DateTime.Now,
                        Owner = this.UserProfile,
                        Url = Dropbox.Download(paths[0]),
                        ThumbnailUrl = Dropbox.Download(paths[1], "Thumbnails")
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
            var photoModels = this.UserProfile.Photos.AsQueryable()
                .OrderByDescending(p => p.CreatedOn)
                .Project()
                .To<PhotoViewModel>();

            return this.View(photoModels);
        }
    }
}
