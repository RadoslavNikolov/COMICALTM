namespace Contests.Models.Strategies.VotingStrategy
{
    using System.Collections.Generic;
    using Interfaces;
    using Microsoft.AspNet.Identity;

    public class ClosedVotingStrategy : IVotingStrategy
    {
        public ClosedVotingStrategy(ICollection<IUser> participants)
        {
            this.Participants = participants;
        }

        public ICollection<IUser> Participants { get; set; }

        public bool CanVote(IUser user)
        {
            return this.Participants.Contains(user);
        }
    }
}
