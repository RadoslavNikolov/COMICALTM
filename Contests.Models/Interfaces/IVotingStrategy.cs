namespace Contests.Models.Interfaces
{
    using Microsoft.AspNet.Identity;

    public interface IVotingStrategy
    {
        bool CanVote(IUser user);
    }
}
