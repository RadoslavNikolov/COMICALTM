namespace Contests.Models.Strategies.VotingStrategy
{
    public class OpenVotingStrategy : VotingStrategy
    {
        public OpenVotingStrategy(Contest contest, Photo photo, string userId)
            : base(contest, photo, userId)
        {
        }
    }
}
