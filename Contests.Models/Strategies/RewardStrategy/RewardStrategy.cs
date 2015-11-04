namespace Contests.Models.Strategies.RewardStrategy
{
    using System.Collections.Generic;
    using Enums;
    using Interfaces;

    public abstract class RewardStrategy
    {
        private ICollection<Contest> contests;

        protected RewardStrategy()
        {
            this.contests = new HashSet<Contest>();
        }

        public int Id { get; set; }

        public RewardType RewardType { get; set; }

        public virtual ICollection<Contest> Contests
        {
            get { return this.contests; }
            set { this.contests = value; }
        }

        public abstract IEnumerable<Photo> DetermineWinners(Contest contest);
    }
}
