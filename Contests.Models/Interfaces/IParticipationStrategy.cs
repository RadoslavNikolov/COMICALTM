namespace Contests.Models.Interfaces
{
    using Microsoft.AspNet.Identity;

    public interface IParticipationStrategy
    {
        bool CanParticipate(IUser user);
    }
}