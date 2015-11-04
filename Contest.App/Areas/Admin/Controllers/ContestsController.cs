namespace Contests.App.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contests.Models;
    using Contests.Models.Enums;
    using Data.UnitOfWork;
    using Helpers;
    using Infrastructure;
    using Models.BindingModels;
    using Models.ViewModels;
    using MvcPaging;
    using Toastr;

    public class ContestsController : BaseAdminController
    {
        public ContestsController(IContestsData data)
            : base(data)
        {
        }

        // GET: Admin/Contests
        public ActionResult Index(string contest_title, int? page)
        {
            this.TempData["contest_title"] = contest_title;
            int currentPageIndex = page ?? 1;

            var contestsQuery = this.ContestsData.Contests.All();

            if (!string.IsNullOrWhiteSpace(contest_title))
            {
                contestsQuery = contestsQuery.Where(c => c.Title.Contains(contest_title));
            }

            Mapper.CreateMap<Contest, ContestPreviewViewModel>()
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.Finalized, opts => opts.MapFrom(src => src.IsFinalized ? "Finalized" : "Not finalized"))
                .ForMember(dest => dest.Organizator, opts => opts.MapFrom(src => src.Organizator.FullName))
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category.Name));

            var contests = contestsQuery
                    .OrderBy(c => c.Title)
                    .Project()
                    .To<ContestPreviewViewModel>()
                    .ToPagedList(currentPageIndex, AppConfig.AdminPanelPageSize);

            return View(contests);
        }

        // GET: Admin/Contests/Edit/id
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

                return this.RedirectToAction("Index");
            }

            if (contest.IsFinalized && !contest.IsActive)
            {
                this.AddToastMessage("Warning", "You are not allowed to edit this contest.", ToastType.Warning);

                return this.RedirectToAction("Index");
            }
                
            return View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ContestBindingModel model)
        {
            if (this.ModelState != null && this.ModelState.IsValid)
            {
                var contest = this.ContestsData.Contests.Find(id);

                //ICollection<User> voters = model.VotingType == VotingType.Close ? this.GetUsers(model.Voters) : new HashSet<User>();
                //ICollection<User> participants = model.ParticipationType == ParticipationType.Close ? this.GetUsers(model.Participants) : new HashSet<User>();

                contest.Title = model.Title;
                contest.Description = model.Description;
                contest.RewardType = model.RewardType;
                contest.DeadLine = model.DeadLine;
                contest.ParticipationType = model.ParticipationType;
                contest.VotingType = model.VotingType;
                //contest.Voters = voters;
                contest.WinnersCount = model.WinnersCount;
                contest.ParticipantsNumberDeadline = model.ParticipantsNumberDeadline;
                //contest.Participants = participants;
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
                
                //this.ContestsData.Contests.Add(contest);
                this.ContestsData.SaveChanges();
                this.AddToastMessage("Success", "Contest edited.", ToastType.Success);
                return this.RedirectToAction("Index", "Contests", routeValues: new { area = "Admin" });
            }

            return this.View(model);
        }

        private void FillVoters(Contest contest, ICollection<string> votersIds, VotingType votingType )
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
    }
}