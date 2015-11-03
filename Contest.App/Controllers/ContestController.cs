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
    using Infrastructure.UserIdProvider;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using Toastr;
    using Validators;

    [Authorize]
    public class ContestController : BaseController
    {
        public ContestController(IContestsData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }


        // <summary>
        /// This action take all or latest(5) active contests
        /// </summary>
        /// <param name="latest">This argument show if we want all or latest active contests</param>
        /// <returns>Return default vew for this action</returns>
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
        public ActionResult Details(int? id)
        {
            var contest = this.ContestsData.Contests.All()
                .Where(c => c.Id == id)
                .Project()
                .To<ContestViewModel>();

            if (contest == null)
            {
                this.AddToastMessage("Info", "No such contest found", ToastType.Info);
                return RedirectToAction("Index", "Home", routeValues: new { area = "" });
            }

            return this.View(contest);

        }

        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContestBindingModel model)
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

                if (model.Upload != null && model.Upload.ContentLength > 0)
                {
                    var wallpaperPaths = Helpers.UploadImages.UploadImage(model.Upload, true);
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
                }

                this.ContestsData.Contests.Add(contest);
                this.ContestsData.SaveChanges();
                this.AddToastMessage("Success", "Contest created.", ToastType.Success);
                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }

            return this.View();
        }

        [HttpGet]
        public ActionResult Edit(int? contestId)
        {
            var contest = this.ContestsData.Contests.Find(contestId);

            if (contest == null)
            {
                this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);
                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }

            var model = ContestBindingModel.CreateFromContest(contest);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContestBindingModel model)
        {
            
            if (this.ModelState != null && this.ModelState.IsValid)
            {
                var contest = this.ContestsData.Contests.Find(model.ContestId);

                ICollection<User> voters = model.VotingType == VotingType.Close ? this.GetUsers(model.Voters) : new HashSet<User>();
                ICollection<User> participants = model.ParticipationType == ParticipationType.Close ? this.GetUsers(model.Participants) : new HashSet<User>();

                contest.Title = model.Title;
                contest.Description = model.Description;
                contest.RewardType = model.RewardType;
                contest.DeadLine = model.DeadLine;
                contest.ParticipationType = model.ParticipationType;
                contest.VotingType = model.VotingType;
                contest.Voters = voters;
                contest.WinnersCount = model.WinnersNumber;
                contest.ParticipantsNumberDeadline = model.ParticipantsNumberDeadline;
                contest.Participants = participants;
                contest.DeadLine = model.DeadLine;
                contest.CategoryId = model.Category;

                if (model.Upload != null && model.Upload.ContentLength > 0)
                {
                    var wallpaperPaths = Helpers.UploadImages.UploadImage(model.Upload, true);
                    var wallpaperUrl = Dropbox.Download(wallpaperPaths[0]);
                    var wallpaperThumbUrl = Dropbox.Download(wallpaperPaths[1], "Thumbnails");

                    contest.WallpaperPath = wallpaperPaths[0];
                    contest.WallpaperUrl = wallpaperUrl;
                    contest.WallpaperThumbPath = wallpaperPaths[1];
                    contest.WallpaperThumbUrl = wallpaperThumbUrl;
                }


                //this.ContestsData.Contests.Add(contest);
                this.ContestsData.SaveChanges();
                this.AddToastMessage("Success", "Contest edited.", ToastType.Success);
                return this.RedirectToAction("Details", "Users", routeValues: new {id = this.UserProfile.Id, area = ""});
            }

            return this.View(model);
        }

        /// <summary>
        /// This action adds vote to concrete Photo 
        /// </summary>
        /// <param name="model">Take arguments need to make successfull vote for concrete Photo </param>
        /// <returns>Return Json result with actual count of votes for this Photo</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Vote(VoteBindingModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var userId = this.User.Identity.GetUserId();

                if (!this.ContestsData.Votes.All().Any(v => v.ContestId == model.ContestId && v.UserId == userId && (v.Photo.OwnerId != userId)))
                {
                    var vote = new Vote
                    {
                        PhotoId = model.PhotoId,
                        UserId = userId,
                        ContestId = model.ContestId                      
                    };
                    this.ContestsData.Votes.Add(vote);
                    this.ContestsData.SaveChanges();

                    var newVotes = this.ContestsData.Votes.All()
                        .Count(v => v.PhotoId == model.PhotoId);
                    return this.Json(newVotes);
                }
            }

            var votes = this.ContestsData.Votes.All().Count(v => v.PhotoId== model.PhotoId);
            return this.Json(votes);
        }

        
        public ActionResult GetAllCategories(int? catId, int? selectedId)
        {
            var categories = this.ContestsData.Categories.All()
                .Where(c => c.IsActive && c.Id != catId)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Selected = (c.Id == selectedId),
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

        public ActionResult Finalize(int contestid)
        {
            var contest = this.IsContestClosable(contestid);

            if (contest == null)
            {
                this.AddToastMessage("Error", "Error with closing this contest!", ToastType.Error);
                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }

            //TODO  Implement the rest of finalization method


            this.AddToastMessage("Success", "You finalized this contest successfully!", ToastType.Success);
            return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });

        }

        public ActionResult Dismiss(int contestid)
        {
            var contest = this.IsContestClosable(contestid);

            if (contest == null)
            {
                this.AddToastMessage("Error", "Error with closing this contest!", ToastType.Error);
                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }

            //contest.IsActive = false;
            //this.ContestsData.SaveChanges();

            this.AddToastMessage("Success", "You dismissed this contest successfully!", ToastType.Success);
            return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
        }

        private Contest IsContestClosable(int contestId)
        {
            var userId = this.UserProfile.Id;
            var isAdmin = this.IsAdmin();
            var contest = this.ContestsData.Contests.All()
                .FirstOrDefault(c => c.Id == contestId && c.IsActive && (c.OrganizatorId == userId || isAdmin));

            return contest;
        }
    }
}