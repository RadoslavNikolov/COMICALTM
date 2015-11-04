namespace Contests.Common
{
    using Models;

    public class VotingModel
    {
        public Contest Contest { get; set; }

        public Photo Photo { get; set; }

        public string UserId { get; set; }
    }
}
