namespace Contests.App.Models.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Contests.Models.Enums;

    public class ContestBindingModel
    {
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

        public string[] Participants { get; set; }

        public DateTime? DeadLine { get; set; }

        public string[] Voters { get; set; }

        public int Category { get; set; }
    }
}