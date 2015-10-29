namespace Contests.Models.Strategies.VotingStrategy
{
    using Interfaces;
    using Microsoft.AspNet.Identity;

    public class OpenVotingStrategy : IVotingStrategy
    {
        public bool CanVote(IUser user)
        {
            return true;
        }
    }
}
