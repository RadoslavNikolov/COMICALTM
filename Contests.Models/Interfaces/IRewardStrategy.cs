namespace Contests.Models.Interfaces
{
    using System.Collections.Generic;

    public interface IRewardStrategy
    {
        IEnumerable<Photo> Determineinners(IList<Photo> photos);
    }
}
