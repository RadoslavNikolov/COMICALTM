namespace Contests.Models.Strategies.DeadlineStrategy
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;

    public class ByParticipantsNumber : DeadlineStrategy
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
