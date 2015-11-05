namespace Contests.App.Models.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Contests.Models;
    using Contests.Models.Enums;
    using Validators;

    public class ContestBindingModel
    {
        public int Id { get; set; }
       
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public RewardType RewardType { get; set; }

        [Required]
        public VotingType VotingType { get; set; }

        [Required]
        public DeadlineType DeadlineType { get; set; }

        [Required]
        public ParticipationType ParticipationType { get; set; }

        public byte? WinnersNumber { get; set; }

        public int? ParticipantsNumberDeadline { get; set; }

        public ICollection<string> Participants { get; set; }

        public DateTime? DeadLine { get; set; }

        public ICollection<string> Voters { get; set; }

        public int Category { get; set; }

        public string WallpaperPath { get; set; }

        public string WallpaperUrl { get; set; }

        public string WallpaperThumbPath { get; set; }

        public string WallpaperThumbUrl { get; set; }

        public bool IsActive { get; set; }

        public bool IsFinalized { get; set; }

        [ValidateImage(ErrorMessage = "Please select an image smaller than 4MB")]
        public HttpPostedFileBase Upload { get; set; }
    }
}