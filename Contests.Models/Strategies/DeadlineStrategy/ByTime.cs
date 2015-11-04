namespace Contests.Models.Strategies.DeadlineStrategy
{
    using System;

    public class ByTime : DeadlineStrategy
    {
        public ByTime(DateTime deadline)
        {
            this.DeadLine = deadline;
        }

        public DateTime DeadLine { get; set; }

        public bool HasFinished()
        {
            return DateTime.Now >= this.DeadLine;
        }
    }
}
