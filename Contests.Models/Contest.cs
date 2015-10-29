namespace Contests.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Contest
    {
        private ICollection<Photo> photos;
        private ICollection<User> participants;
        private ICollection<User> voters;
        private ICollection<User> winners;
        
        public Contest()
        {
            this.photos = new HashSet<Photo>();
            this.participants = new HashSet<User>();
            this.voters = new HashSet<User>();
            this.winners = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string FounderId { get; set; }

        public virtual User Founder { get; set; }

        public int RewardStrategyId { get; set; }

        public RewardType RewardType { get; set; }

        public DeadlineType DeadlineType { get; set; }

        public ParticipationType ParticipationType { get; set; }

        public VotingType VotingType { get; set; }

        public virtual ICollection<Photo> Photos
        {
            get { return this.photos; }
            set { this.photos = value; }
        }

        public virtual ICollection<User> Winners
        {
            get { return this.winners; }
            set { this.winners = value; }
        }

        public virtual ICollection<User> Participants
        {
            get { return this.participants; }
            set { this.participants = value; }
        }

        public virtual ICollection<User> Voters
        {
            get { return this.voters; }
            set { this.voters = value; }
        }
    }
}
