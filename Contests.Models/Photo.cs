namespace Contests.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        private ICollection<Vote> votes;

        public Photo()
        {
            this.votes = new HashSet<Vote>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        [StringLength(255)]
        public string ThumbnailUrl { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        [StringLength(255)]
        public string ThumbPath { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        [Required]
        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsWinner { get; set; }

        public virtual ICollection<Vote> Votes
        {
            get { return this.votes; }
            set { this.votes = value; }
        }
    }
}