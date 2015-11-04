namespace Contests.Models.Strategies.RewardStrategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SingleWinner : RewardStrategy
    {
        private const int WinnersCount = 1;

        public override void DetermineWinners(Contest contest)
        {
            var winningPhoto = contest.Photos
                .Where(p => p.IsDeleted == false && p.Votes.Count > 0)
                .OrderByDescending(p => p.Votes.Count)
                .ThenBy(p => p.CreatedOn)
                .Take(WinnersCount)
                .ToList();

            winningPhoto.First().IsWinner = true;
            ICollection<User> winners = winningPhoto.Select(p => p.Owner).ToList();
            contest.Winners = winners;
            contest.WinningPhotosId = winningPhoto.Select(p => p.Id).ToList();
            contest.IsFinalized = true;
            contest.IsActive = false;
            contest.FinishedOn = DateTime.Now;
            contest.WallpaperUrl = winningPhoto.First().Url;
        }
    }
}