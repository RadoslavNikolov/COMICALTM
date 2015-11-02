namespace Contests.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Contests.Models;
    using Contests.Models.Enums;
    using Contests.Models.Strategies.RewardStrategy;
    using Data.UnitOfWork;
    using Helpers;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using Toastr;

    [Authorize]
    public class ContestController : BaseController
    {
        public ContestController(IContestsData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult My()
        {
            var currentUserId = this.UserProfile.Id;
            var contests = this.ContestsData.Contests.All()
               .Where(c => c.OrganizatorId == currentUserId && c.IsActive)
               .OrderByDescending(c => c.CreatedOn)
               .Project()
               .To<ContestViewModel>();

            if (!contests.Any())
            {
                this.AddToastMessage("Info", "No active contests created by you", ToastType.Info);
                return this.RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(contests);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult All(bool latest)
        {
            var contest = this.ContestsData.Contests.All()
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedOn)
                .Project()
                .To<ContestViewModel>();

            if (latest)
            {
                this.ViewBag.Latest = "Latest contests";
                contest = contest.Take(5);
            }

            this.ViewBag.Latest = "All contests";
            return View(contest);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var contest = this.ContestsData.Contests.All()
                .Where(c => c.Id == id)
                .Project()
                .To<ContestViewModel>();

            if (contest == null)
            {
                this.AddToastMessage("Info", "No such contest found", ToastType.Info);
                return RedirectToAction("Index", "Home");
            }

            return this.View(contest);

        }

        [HttpGet]
        [Authorize]
        public ActionResult Upload(int contestId)
        {
            var contest = this.ContestsData.Contests.Find(contestId);

            if (contest == null)
            {
                this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);
                return this.RedirectToAction("Index", "Contest");
            }

            var model = PhotoBindingModel.CreateFrom(contest);

            return View(model);

        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(PhotoBindingModel model, int contestId)
        {
            var test = contestId;

            if (ModelState.IsValid)
            {
                if (model.Upload != null)
                {
                    var paths = Helpers.UploadImages.UploadImage(model.Upload, false);
                    var contest = this.ContestsData.Contests.Find(contestId);

                    if (contest == null)
                    {
                        this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);
                        return this.RedirectToAction("Index", "Contest");
                    }

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

                return this.RedirectToAction("Details", routeValues: new {id = contestId});
            }

            this.AddToastMessage("Error", "Error while uploading", ToastType.Error);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid model");
        }
        

        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContestBindingModel model, HttpPostedFileBase upload)
        {
            if (this.ModelState != null && this.ModelState.IsValid)
            {
                ICollection<User> voters = model.VotingType == VotingType.Close ? this.GetUsers(model.Voters) : new HashSet<User>();
                ICollection<User> participants = model.ParticipationType == ParticipationType.Close ? this.GetUsers(model.Participants) : new HashSet<User>();

                var contest = new Contest
                {
                    Title = model.Title,
                    Description = model.Description,
                    OrganizatorId = this.UserProfile.Id,
                    RewardType = model.RewardType,
                    DeadlineType = model.DeadlineType,
                    ParticipationType = model.ParticipationType,
                    VotingType = model.VotingType,
                    Voters = voters,
                    WinnersCount = model.WinnersNumber,
                    ParticipantsNumberDeadline = model.ParticipantsNumberDeadline,
                    Participants = participants,
                    DeadLine = model.DeadLine,
                    CategoryId = model.Category
                };

                if (upload != null && upload.ContentLength > 0)
                {
                    var wallpaperPaths = Helpers.UploadImages.UploadImage(upload, true);
                    var wallpaperUrl = Dropbox.Download(wallpaperPaths[0]);
                    var wallpaperThumbUrl = Dropbox.Download(wallpaperPaths[1], "Thumbnails");

                    contest.WallpaperPath = wallpaperPaths[0];
                    contest.WallpaperUrl = wallpaperUrl;
                    contest.WallpaperThumbPath = wallpaperPaths[1];
                    contest.WallpaperThumbUrl = wallpaperThumbUrl;
                }
                else
                {
                    contest.WallpaperUrl = AppKeys.WallpaperUrl;
                    contest.WallpaperThumbUrl = AppKeys.WallpaperThumbUrl;
                }

                this.ContestsData.Contests.Add(contest);
                this.ContestsData.SaveChanges();
                this.AddToastMessage("Success", "Contest created.", ToastType.Success);
                return this.RedirectToAction("My");
            }

            return this.View();
        }

        private ICollection<IUser> GetParticipants(ICollection<string> participantsUsernames)
        {
            ICollection<IUser> participants = new List<IUser>();

            if (participantsUsernames == null)
            {
                return participants;
            }

            foreach (var username in participantsUsernames)
            {
                var user = this.ContestsData.Users.All().FirstOrDefault(u => u.UserName == username);
                if (user == null)
                {
                    throw new ArgumentException();
                }

                participants.Add(user);
            }

            return participants;
        }
        
        public ActionResult GetAllCategories(int? catId)
        {
            var categories = this.ContestsData.Categories.All()
                .Where(c => c.IsActive && c.Id != catId)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

            return this.PartialView("Partial/_CategoriesSelect", categories);
        }

        private ICollection<User> GetUsers(string[] usersId)
        {
            ICollection<User> users = new HashSet<User>();

            if (usersId != null)
            {
                foreach (string id in usersId)
                {
                    User wantedUser = this.ContestsData.Users.Find(id);
                    if (wantedUser == null)
                    {
                        throw new NullReferenceException();
                    }

                    users.Add(wantedUser);
                }
            }

            return users;
        }
    }
}