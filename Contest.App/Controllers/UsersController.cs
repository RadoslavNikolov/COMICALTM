using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Contests.Data;
using Contests.Models;

namespace Contests.App.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contest.App;
    using Contests.Data.UnitOfWork;
    using Helpers;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Models.BindingModels;
    using Models.ViewModels;

    [Authorize]
    public class UsersController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private UserManager _userManager;


        public UsersController(IContestsData data, UserManager userManager, ApplicationSignInManager signInManager)
            :this(data)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public UsersController(IContestsData data)
            : base(data)
        {
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public ActionResult All()
        {
            return View(this.ContestsData.Users.All().ToList());
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var users = this.ContestsData.Users.All()
                .Where(u => u.Id != this.UserProfile.Id)
                .OrderBy(u => u.UserName)
                .Project()
                .To<UserInfoViewModel>();

            return this.PartialView("Partial/MultipleSelectPartial", users);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                if (this.Request.IsAuthenticated)
                {
                    id = this.User.Identity.GetUserId();
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
            }
            User user = this.ContestsData.Users.Find(id);

            if (user == null)
            {
                return this.HttpNotFound();
            }

            var viewModel = this.ContestsData.Users.All().Where(u => u.Id == id)
                .Project()
                .To<UserInfoViewModel>().FirstOrDefault();

            if (user.ProfileImage != null)
            {
                viewModel.ProfileImageUrl = Dropbox.Download(user.ProfileImage.Path);
                viewModel.ThumbProfileUrl = Dropbox.Download(user.ProfileImage.ThumbnailPath, "Thumbnails");
            }
            
                 
            return this.View(viewModel);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                this.ContestsData.Users.Add(user);
                this.ContestsData.SaveChanges();
                return RedirectToAction("All");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit()
        {
            var userName = this.User.Identity.Name;
            var user = this.ContestsData.Users.All().Where(u => u.UserName == userName)
                .Project()
                .To<UserEditViewModel>().FirstOrDefault();

            if (user != null && user.ProfileImage != null)
            {
                user.ProfileImageUrl = Dropbox.Download(user.ProfileImage.Path);
                user.ThumbsProfileImageUrl = Dropbox.Download(user.ProfileImage.ThumbnailPath, "Thumbnails");
            }         
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditBindingModel model)
        {
 
            var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            //user.Fullname = model.Fullname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            //user.Files.Add(model.Upload);


            if (model.Upload != null && model.Upload.ContentLength > 0)
            {

                var paths = Helpers.UploadImages.UploadImage(model.Upload, true);

                var newPhoto = new Photo
                {
                    CreatedOn = DateTime.Now,
                    Owner = user,
                    Path = paths[0],
                    ThumbnailPath = paths[1]
                };

                user.ProfileImage = newPhoto;
            }
          
            var result = await this.UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {                  
                this.TempData["Success"] = new[] {"Edit successfull"};
                model.ProfileImage = user.ProfileImage;
                model.ProfileImageUrl = Dropbox.Download(user.ProfileImage.Path);
                model.ThumbsProfileImageUrl = Dropbox.Download(user.ProfileImage.ThumbnailPath, "Thumbnails");

            }
            else
            {
                this.AddErrors(result);
            }

            return View(model);
        }

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(UserEditBindingModel model, HttpPostedFileBase upload)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var userId = this.User.Identity.GetUserId();
        //    var user = this.ContestsData.Users.Find(userId);

        //    user.Email = model.Email;
        //    user.UserName = model.UserName;
        //    user.PhoneNumber = model.PhoneNumber;

        //    if (upload != null && upload.ContentLength > 0)
        //    {
        //        var photo = new File
        //        {
        //            FileName = System.IO.Path.GetFileName(upload.FileName),
        //            FileType = FileType.Photo,
        //            ContentType = upload.ContentType,
        //            UserId = user.Id
        //        };

        //        using (var render = new System.IO.BinaryReader(upload.InputStream))
        //        {
        //            photo.Content = render.ReadBytes(upload.ContentLength);
        //        }

        //        user.Files = new List<File> { photo };
        //    }

        //    this.ContestsData.Users.Update(user);
        //    this.ContestsData.SaveChanges();
        //    this.TempData["success"] = new[] { "Successfully update your profile!" };

        //    return View(user);
        //}

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = this.ContestsData.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = this.ContestsData.Users.Find(id);
            this.ContestsData.Users.Remove(user);
            this.ContestsData.SaveChanges();
            return RedirectToAction("All");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
