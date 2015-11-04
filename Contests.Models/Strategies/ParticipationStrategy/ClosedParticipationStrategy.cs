namespace Contests.Models.Strategies.ParticipationStrategy
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;

    public class ClosedParticipationStrategy : ParticipationStrategy
    {
        public ClosedParticipationStrategy(ICollection<IUser> participants)
        {
            this.Participants = participants;
        }

        public ICollection<IUser> Participants { get; set; }

        public bool CanParticipate(IUser participant)
        {
            return this.Participants.Contains(participant);
        }
    }
}
