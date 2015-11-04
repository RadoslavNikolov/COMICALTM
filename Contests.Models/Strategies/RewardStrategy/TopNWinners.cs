namespace Contests.Models.Strategies.RewardStrategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class TopNWinners : RewardStrategy
    {
        public TopNWinners(byte winnersCount)
        {
            this.WinnersCount = winnersCount;
        }

        public byte WinnersCount { get; set; }

        public override IEnumerable<Photo> DetermineWinners(Contest contest)
        {
            IEnumerable<Photo> winners = contest.Photos.OrderByDescending(p => p.Votes.Count).Take(this.WinnersCount);

            return winners;
        }
    }
}
