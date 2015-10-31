namespace Contests.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Contests.Models;
    using Contests.Models.Strategies.RewardStrategy;
    using Data.UnitOfWork;
    using Helpers;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;

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

        public ActionResult CreateContest(ContestBindingModel model, HttpPostedFileBase upload)
        {
            var contest = new Contest
            {
                Title = model.Title,
                Description = model.Description,
                OrganizatorId = this.UserProfile.Id,
                RewardType = model.RewardType,
                DeadlineType = model.DeadlineType,
                ParticipationType = model.ParticipationType,
                VotingType = model.VotingType,
                Voters = model.Voters,
                WinnersCount = model.WinnersNumber,
                ParticipantsNumberDeadline = model.ParticipantsNumberDeadline,
                DeadLine = model.DeadLine
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

            this.ContestsData.Contests.Add(contest);
            this.ContestsData.SaveChanges();

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
    }
}