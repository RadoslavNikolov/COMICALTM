using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contests.App.Controllers
{
    using System.Net;
    using Contests.Models;
    using Data.UnitOfWork;
    using Helpers;
    using Models.BindingModels;
    using Toastr;
    using Validators;

    [Authorize]
    public class PhotoController : BaseController
    {
        public PhotoController(IContestsData data) : base(data)
        {
        }

        [HttpGet]
        public ActionResult Upload(int contestId)
        {
            var contest = this.ContestsData.Contests.Find(contestId);

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
        public ActionResult Upload(PhotoBindingModel model, int contestId)
        {
            if (this.ModelState != null && this.ModelState.IsValid)
            {
                if (model.Upload != null)
                {
                    var contest = CustomValidators.IsContestValid(this.ContestsData, this.UserProfile, contestId);

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
                        ContestId = contestId
                    };

                    contest.Photos.Add(newPhoto);
                    this.ContestsData.SaveChanges();
                    //this.TempData["Success"] = new[] { "Upload successfull" };
                    this.AddToastMessage("Success", "Photo uploaded.", ToastType.Success);
                }

                return this.RedirectToAction("Details", "Contest", routeValues: new { id = contestId });
            }

            this.AddToastMessage("Error", "Something went wrong while uploading!", ToastType.Error);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid model");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int photoId, int contestId)
        {
            var photoToDel = this.ContestsData.Photos.Find(photoId);

            if (photoToDel.OwnerId == this.UserProfile.Id && photoToDel.ContestId == contestId)
            {
                this.ContestsData.Photos.Remove(photoToDel);
                this.ContestsData.SaveChanges();
                this.AddToastMessage("Success", "You deleted this contest successfully!", ToastType.Success);
                return this.RedirectToAction("Details", "Contest", routeValues: new { id = contestId, area = "" });
            }

            this.AddToastMessage("Error", "Something went wrong during deletion", ToastType.Error);
            return this.RedirectToAction("Details", "Contest", routeValues: new { id = contestId, area = "" });
        }    
    }
}