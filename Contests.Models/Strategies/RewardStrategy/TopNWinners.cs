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

        public override IEnumerable<Photo> DetermineWinners(IList<Photo> photos)
        {
            IEnumerable<Photo> winners = photos.OrderByDescending(p => p.Votes.Count).Take(this.WinnersCount);

            return winners;
        }
    }
}
