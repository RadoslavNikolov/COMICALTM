namespace Contests.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Contests.Models;
    using Contests.Models.Strategies.RewardStrategy;
    using Data.UnitOfWork;
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

        public ActionResult CreateContest(ContestBindingModel model)
        {
            ICollection<IUser> participants = new List<IUser>();
            User user = this.ContestsData.Users.All().FirstOrDefault(u => u.UserName == "martin");
            participants.Add(user);
            RewardStrategy rewardStrategy = new SingleWinner();

            var contest = new Contest
            {
                Title = "Zalgavie",
                Description = "opisanie",
                StartDate = DateTime.Now,
                FounderId = this.UserProfile.Id
            };

            this.ContestsData.Contests.Add(contest);
            this.ContestsData.SaveChanges();

            return null;




            //if (!this.ModelState.IsValid)
            //{
            //    return this.View(model);
            //}

            //ICollection<IUser> participants = new List<IUser>();
            //if (model.ParticipationType == ParticipationType.Close)
            //{
            //    participants = this.GetParticipants(model.Participants);
            //}

            //IParticipationStrategy participationStrategy = ParticipationFactory.CreateStrategy(model.ParticipationType, participants);

            //var contest = new Contest
            //{
            //    Title = model.Title,
            //    Description = model.Description,
            //    StartDate = model.StartDate,
            //    FounderId = this.UserProfile.Id,
            //    ParticipationStrategy = participationStrategy
            //};

            //return null;
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