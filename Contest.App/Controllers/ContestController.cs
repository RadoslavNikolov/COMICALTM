namespace Contests.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Common.Factories;
    using Contests.Models;
    using Contests.Models.Enums;
    using Contests.Models.Strategies.RewardStrategy;
    using Contests.Models.Strategies.VotingStrategy;
    using Data.UnitOfWork;
    using Helpers;
    using Hubs;
    using Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Models.BindingModels;
    using Models.ViewModels;
    using PagedList;
    using Toastr;
    using Validators;
    using IUserIdProvider = Infrastructure.UserIdProvider.IUserIdProvider;

    [System.Web.Mvc.Authorize]
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
        public ActionResult All(int? page)
        {
            var contest = this.ContestsData.Contests.All()
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedOn)
                .Project()
                .To<ContestViewModel>()
                .ToPagedList(pageNumber: page ?? 1, pageSize: AppConfig.AdminPanelPageSize);

            this.ViewBag.Title = "All contests";
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
                return this.RedirectToAction("Index", "Home", routeValues: new { area = "" });
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
                Category category = this.ContestsData.Categories.Find(model.Category);
                bool isModelValid = CustomValidators.IsContestModelValid(category, model);
                if (!isModelValid)
                {
                    this.AddToastMessage("Error", "Invalid data.", ToastType.Error);
                    return View(model);
                }

                ICollection<User> voters = model.VotingType == VotingType.Closed ? this.GetUsers(model.Voters) : new HashSet<User>();
                ICollection<User> participants = model.ParticipationType == ParticipationType.Closed ? this.GetUsers(model.Participants) : new HashSet<User>();

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
        public ActionResult Edit(int id)
        {
            Mapper.CreateMap<Contest, ContestBindingModel>()
                .ForMember(dest => dest.Participants, opts => opts.MapFrom(src => src.Participants.Select(p => p.Id)))
                .ForMember(dest => dest.Voters, opts => opts.MapFrom(src => src.Voters.Select(v => v.Id)))
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category.Id));

            var contest = this.ContestsData.Contests.All()
                .Where(c => c.Id == id)
                .Project()
                .To<ContestBindingModel>()
                .FirstOrDefault();

            if (contest == null)
            {
                this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);
                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }


            if (contest.IsFinalized && !contest.IsActive)
            {
                this.AddToastMessage("Warning", "You are not allowed to edit this contest.", ToastType.Warning);

                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }

            return this.View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ContestBindingModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var contest = this.ContestsData.Contests.Find(id);
                
                contest.Title = model.Title;
                contest.Description = model.Description;
                contest.RewardType = model.RewardType;
                contest.DeadLine = model.DeadLine;
                contest.ParticipationType = model.ParticipationType;
                contest.VotingType = model.VotingType;
                contest.WinnersCount = model.WinnersNumber;
                contest.ParticipantsNumberDeadline = model.ParticipantsNumberDeadline;
                contest.DeadLine = model.DeadLine;
                contest.CategoryId = model.Category;

                this.FillVoters(contest, model.Voters, model.VotingType);
                this.FillPacticipants(contest, model.Participants, model.ParticipationType);

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

                var contest = this.ContestsData.Contests.Find(model.ContestId);
                if (contest == null)
                {
                    this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);
                    var currentVotesNumber = this.ContestsData.Votes.All().Count(v => v.PhotoId == model.PhotoId && !v.Photo.IsDeleted);
                    return this.Json(currentVotesNumber);
                }

                if (contest.IsFinalized && !contest.IsActive)
                {
                    this.AddToastMessage("Warning", "You are not allowed to vote for this contest.", ToastType.Warning);
                    var currentVotesNumber = this.ContestsData.Votes.All().Count(v => v.PhotoId == model.PhotoId && !v.Photo.IsDeleted);
                    return this.Json(currentVotesNumber);
                }

                var photo = this.ContestsData.Photos.Find(model.PhotoId);
                if (photo == null)
                {
                    this.AddToastMessage("Error", "Non existing photo!", ToastType.Error);
                    var currentVotesNumber = this.ContestsData.Votes.All().Count(v => v.PhotoId == model.PhotoId && !v.Photo.IsDeleted);
                    return this.Json(currentVotesNumber);
                }

                VotingModel votingModel = new VotingModel { Contest = contest, Photo = photo, UserId = userId };
                VotingStrategy strategy = VotingFactory.CreateStrategy(contest.VotingType, votingModel);
                bool canVote = strategy.CanVote();
                if (canVote)
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
                        .Count(v => v.PhotoId == model.PhotoId && !v.Photo.IsDeleted);
                    return this.Json(newVotes);
                }
            }

            var votes = this.ContestsData.Votes.All().Count(v => v.PhotoId== model.PhotoId && !v.Photo.IsDeleted);
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

        [HttpGet]
        public ActionResult Finalize(int id)
        {
            var contest = this.ContestsData.Contests.All()
                .Where(c => c.Id == id)
                .Project()
                .To<ContestViewModel>()
                .FirstOrDefault();

            if (contest == null)
            {
                this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);

                return this.RedirectToAction("Details", "Contest", routeValues: new { area = "" });
            }

            return View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Finalize")]
        public ActionResult FinalizePost(int id)
        {
            var contest = this.IsContestClosable(id);
            
            if (contest == null)
            {
                this.AddToastMessage("Error", "Error with closing this contest!", ToastType.Error);
                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }

            RewardModel model = new RewardModel { WinnersCount = contest.WinnersCount };

            RewardStrategy strategy = RewardFactory.CreateStrategy(contest.RewardType, model);
            strategy.DetermineWinners(contest);

            this.ContestsData.SaveChanges();

            this.SendParticipantsNotification(contest);

            this.AddToastMessage("Success", "You finalized this contest successfully!", ToastType.Success);
            return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
        }
        
        [HttpGet]
        public ActionResult Dismiss(int id)
        {
            var contest = this.ContestsData.Contests.All()
                .Where(c => c.Id == id)
                .Project()
                .To<ContestViewModel>()
                .FirstOrDefault();

            if (contest == null)
            {
                this.AddToastMessage("Error", "Non existing contest!", ToastType.Error);

                return this.RedirectToAction("Details", "Contest", routeValues: new { area = "" });
            }

            return View(contest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Dismiss")]
        public ActionResult DismissPost(int id)
        {
            var contest = this.IsContestClosable(id);

            if (contest == null)
            {
                this.AddToastMessage("Error", "Error with closing this contest!", ToastType.Error);
                return this.RedirectToAction("Details", "Users", routeValues: new { id = this.UserProfile.Id, area = "" });
            }

            contest.IsActive = false;
            this.ContestsData.SaveChanges();

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

        private void FillVoters(Contest contest, ICollection<string> votersIds, VotingType votingType)
        {
            if (votersIds == null || votingType == VotingType.Open)
            {
                contest.Voters.Clear();
            }
            else
            {
                var voters = contest.Voters.ToList();

                foreach (var voterId in votersIds)
                {
                    if (contest.Voters.All(v => v.Id != voterId))
                    {
                        var voter = this.ContestsData.Users.Find(voterId);
                        contest.Voters.Add(voter);
                    }
                }

                foreach (var voter in voters)
                {
                    if (!votersIds.Contains(voter.Id))
                    {
                        contest.Voters.Remove(voter);
                    }
                }
            }
        }

        private void FillPacticipants(Contest contest, ICollection<string> participantsIds, ParticipationType participationType)
        {
            if (participantsIds == null || participationType == ParticipationType.Open)
            {
                contest.Participants.Clear();
            }
            else
            {
                var participants = contest.Participants.ToList();

                foreach (var participantId in participantsIds)
                {
                    if (contest.Participants.All(p => p.Id != participantId))
                    {
                        var participant = this.ContestsData.Users.Find(participantId);
                        contest.Participants.Add(participant);
                    }
                }

                foreach (var participant in participants)
                {
                    if (!participantsIds.Contains(participant.Id))
                    {
                        contest.Participants.Remove(participant);
                    }
                }
            }
        }

        private void SendParticipantsNotification(Contest contest)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();

            var participants = contest.Photos.GroupBy(f => f.OwnerId);

            foreach (var participant in participants)
            {
                var notification = new Notification
                {
                    UserId = participant.Key,
                    Message = string.Format("A contest you participated in was finalized. See contest result <a href=\"Winners/Details/{0}\">here</a>", contest.Id),
                    Date = DateTime.Now,
                    IsRead = false
                };

                this.ContestsData.Notifications.Add(notification);
                hubContext.Clients.Group(participant.Key).receiveNotification();
            }

            this.ContestsData.SaveChanges();
        }
    }
}