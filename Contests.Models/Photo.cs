﻿namespace Contests.Models
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

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Vote> Votes
        {
            get { return this.votes; }
            set { this.votes = value; }
        }
    }
}