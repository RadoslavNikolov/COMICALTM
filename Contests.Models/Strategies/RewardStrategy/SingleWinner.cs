namespace Contests.Models.Strategies.RewardStrategy
{
    using System.Collections.Generic;
    using System.Linq;

    public class SingleWinner : RewardStrategy
    {
        public override IEnumerable<Photo> DetermineWinners(IList<Photo> photos)
        {
            IEnumerable<Photo> winner = photos.OrderByDescending(p => p.Votes.Count).FirstOrDefault() as IEnumerable<Photo>;

            return winner;
        }
    }
}