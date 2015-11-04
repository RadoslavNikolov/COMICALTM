namespace Contests.Models.Strategies.VotingStrategy
{
    using System.Linq;

    public class ClosedVotingStrategy : VotingStrategy
    {
        public ClosedVotingStrategy(Contest contest, Photo photo, string userId)
            : base(contest, photo, userId)
        {
        }

        public override bool CanVote()
        {
            bool canVote = this.Contest.Voters.Any(v => v.Id == this.UserId);
            if (!canVote || base.CanVote() == false)
            {
                return false;
            }

            return true;
        }
    }
}
