namespace Contests.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Contest
    {
        private ICollection<Photo> photos;
        private ICollection<User> winners;
        private ICollection<Vote> votes;
        
        public Contest()
        {
            this.photos = new HashSet<Photo>();
            this.Participants = new HashSet<string>();
            this.Voters = new HashSet<string>();
            this.winners = new HashSet<User>();
            this.votes = new HashSet<Vote>();
            this.IsActive = true;
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public string WallpaperPath { get; set; }

        public string WallpaperUrl { get; set; }

        public string WallpaperThumbPath { get; set; }

        public string WallpaperThumbUrl { get; set; }

        [Required]
        public string OrganizatorId { get; set; }

        public virtual User Organizator { get; set; }

        public RewardType RewardType { get; set; }

        public DeadlineType DeadlineType { get; set; }

        public ParticipationType ParticipationType { get; set; }

        public VotingType VotingType { get; set; }

        public byte? WinnersCount { get; set; }

        public int? ParticipantsNumberDeadline { get; set; }

        public DateTime? DeadLine { get; set; }

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

        public ICollection<string> Participants { get; set; }

        public ICollection<string> Voters { get; set; }

        public virtual ICollection<Vote> Votes
        {
            get { return this.votes; }
            set { this.votes = value; }
        }
    }
}
