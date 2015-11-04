namespace Contests.Models.Strategies.RewardStrategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TopNWinners : RewardStrategy
    {
        public TopNWinners(byte winnersCount)
        {
            this.WinnersCount = winnersCount;
        }

        public byte WinnersCount { get; set; }

        public override void DetermineWinners(Contest contest)
        {
            var winningPhotos = contest.Photos
                .Where(p => p.IsDeleted == false && p.Votes.Count > 0)
                .OrderByDescending(p => p.Votes.Count)
                .ThenBy(p => p.CreatedOn)
                .Take(this.WinnersCount)
                .ToList();

            ICollection<User> winners = winningPhotos.Select(p => p.Owner).ToList();
            contest.Winners = winners;
            contest.WinningPhotosId = winningPhotos.Select(p => p.Id).ToList();
            contest.IsFinalized = true;
            contest.IsActive = false;
            contest.FinishedOn = DateTime.Now;
            contest.WallpaperUrl = winningPhotos.First().Url;
        }
    }
}