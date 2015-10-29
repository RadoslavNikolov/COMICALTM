namespace Contests.Models.Strategies.ParticipationStrategy
{
    using Interfaces;
    using Microsoft.AspNet.Identity;

    public class OpenParticipationStrategy : IParticipationStrategy
    {
        public bool CanParticipate(IUser user)
        {
            return true;
        }
    }
}
