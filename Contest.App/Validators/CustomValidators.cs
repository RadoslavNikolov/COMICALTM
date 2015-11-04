namespace Contests.App.Validators
{
    using System;
    using System.Linq;
    using System.Web;
    using Contests.Models;
    using Contests.Models.Enums;
    using Data.UnitOfWork;
    using Microsoft.Ajax.Utilities;

    public static class CustomValidators
    {
        public static Contest IsContestValid(IContestsData context, User creator, int contestId)
        {

            var contest = context.Contests.All()
                .FirstOrDefault(c => c.IsActive && c.Id == contestId);

            if (contest != null)
            {
                if (contest.ParticipationType == ParticipationType.Open && !HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                if (contest.ParticipationType == ParticipationType.Close && !(contest.Participants.Any(p => p.Id == creator.Id)))
                {
                    return null;
                }

                if (contest.DeadlineType == DeadlineType.ByParticipants && (contest.Photos.DistinctBy(p => p.OwnerId).Count() > contest.ParticipantsNumberDeadline))
                {
                    return null;
                }

                if (contest.DeadlineType == DeadlineType.ByTime && DateTime.Now >= contest.DeadLine)
                {
                    return null;
                }
            }

            return contest;
        }
    }
}