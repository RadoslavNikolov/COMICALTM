namespace Contests.Models.Strategies.RewardStrategy
{
    using System.Collections.Generic;
    using System.Linq;

    public class SingleWinner : RewardStrategy
    {
        public override IEnumerable<Photo> DetermineWinners(Contest contest)
        {
            IEnumerable<Photo> winner = contest.Photos.OrderByDescending(p => p.Votes.Count).FirstOrDefault() as IEnumerable<Photo>;

            return winner;
        }
    }
}