using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contests.App.Controllers
{
    using System.Net;
    using AutoMapper.QueryableExtensions;
    using Contests.Models;
    using Data.UnitOfWork;
    using Helpers;
    using Models.BindingModels;
    using Models.ViewModels;
    using Toastr;
    using Validators;

    [Authorize]
    public class PhotoController : BaseController
    {
        public PhotoController(IContestsData data) : base(data)
        {
        }

        [HttpGet]
        public ActionResult Upload(int id)
        {
            var contest = this.ContestsData.Contests.Find(id);

            if (contest == null)
            {
                this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);
                return this.RedirectToAction("Index", "Home", routeValues: new { area = "" });
            }

            var model = PhotoBindingModel.CreateFrom(contest);

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(PhotoBindingModel model, int id)
        {
            if (this.ModelState != null && this.ModelState.IsValid)
            {
                if (model.Upload != null)
                {
                    var contest = CustomValidators.IsContestValid(this.ContestsData, this.UserProfile, id);

                    if (contest == null)
                    {
                        this.AddToastMessage("Error", "Something went wrong while uploading!", ToastType.Error);
                        return this.RedirectToAction("Index", "Home", routeValues: new { area = "" });
                    }

                    var paths = Helpers.UploadImages.UploadImage(model.Upload, false);
                    var newPhoto = new Photo
                    {
                        CreatedOn = DateTime.Now,
                        Owner = this.UserProfile,
                        Path = paths[0],
                        ThumbPath = paths[1],
                        Url = Dropbox.Download(paths[0]),
                        ThumbnailUrl = Dropbox.Download(paths[1], "Thumbnails"),
                        ContestId = id
                    };

                    contest.Photos.Add(newPhoto);
                    this.ContestsData.SaveChanges();
                    //this.TempData["Success"] = new[] { "Upload successfull" };
                    this.AddToastMessage("Success", "Photo uploaded.", ToastType.Success);
                }

                return this.RedirectToAction("Details", "Contest", routeValues: new { id = id });
            }

            this.AddToastMessage("Error", "Something went wrong while uploading!", ToastType.Error);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid model");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var photoToDel = this.ContestsData.Photos.All()
                .Where(p => p.Id == id)
                .Project()
                .To<PhotoViewModel>()
                .FirstOrDefault();

            if (photoToDel == null)
            {
                this.AddToastMessage("Error", "Non existing photo!", ToastType.Error);

                return this.RedirectToAction("Details", "Contest", routeValues: new { area = "" });
            }

            return View(photoToDel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int contestId)
        {
            var photoToDel = this.ContestsData.Photos.Find(id);

            if ((photoToDel.OwnerId == this.UserProfile.Id || this.IsAdmin()) && photoToDel.ContestId == contestId)
            {
                photoToDel.IsDeleted = true;
                //this.ContestsData.Photos.Remove(photoToDel);
                this.ContestsData.SaveChanges();
                this.AddToastMessage("Success", "You deleted this photo successfully!", ToastType.Success);
                return this.RedirectToAction("Details", "Contest", routeValues: new { id = contestId, area = "" });
            }

            this.AddToastMessage("Error", "Something went wrong during deletion", ToastType.Error);
            return this.RedirectToAction("Details", "Contest", routeValues: new { id = contestId, area = "" });
        }    
    }
}