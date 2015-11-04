namespace Contests.Models.Strategies.ParticipationStrategy
{
    using Microsoft.AspNet.Identity;

    public class OpenParticipationStrategy : ParticipationStrategy
    {
        public bool CanParticipate(IUser user)
        {
            return true;
        }
    }
}
