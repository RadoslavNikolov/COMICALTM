namespace Contests.Models.Strategies.DeadlineStrategy
{
    using System.Collections.Generic;
    using Interfaces;
    using Microsoft.AspNet.Identity;

    public class ByParticipantsNumber : IDeadlineStrategy
    {
        public ByParticipantsNumber(int participantsNumber)
        {
            this.ParticipantsNumber = participantsNumber;
        }

        public int ParticipantsNumber { get; set; }

        public ICollection<IUser> Participants { get; set; }

        public bool HasFinished()
        {
            return this.Participants.Count >= this.ParticipantsNumber;
        }
    }
}
