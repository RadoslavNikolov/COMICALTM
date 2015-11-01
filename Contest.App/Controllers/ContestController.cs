namespace Contests.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Contests.Models;
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
            var currentUserId = UserProfile.Id;
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

        public ActionResult CreateContest(ContestBindingModel model, HttpPostedFileBase upload)
        {
            if (this.ModelState != null && this.ModelState.IsValid)
            {
                ICollection<User> voters = this.GetUsers(model.Voters);

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
                    DeadLine = model.DeadLine,
                    CategoryId = Int32.Parse(model.Category)
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
                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        private ICollection<IUser> GetParticipants(ICollection<string> participantsUsernames)
        {
            ICollection<IUser> participants = new List<IUser>();
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

        private ICollection<User> GetUsers(string[] usersId)
        {
            ICollection<User> users = new HashSet<User>();

            foreach (string id in usersId)
            {
                User wantedUser = this.ContestsData.Users.Find(id);
                if (wantedUser == null)
                {
                    throw new NullReferenceException();
                }

                users.Add(wantedUser);
            }

            return users;
        }
    }
}