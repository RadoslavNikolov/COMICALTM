namespace Contests.App.Areas.Admin.Models.ViewModels
{
    using Contests.Models;
    using Contests.Models.Enums;
    using Infrastructure.Mapping;

    public class ContestPreviewViewModel : IMapFrom<Contest>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Organizator { get; set; }

        public RewardType RewardType { get; set; }

        public VotingType VotingType { get; set; }

        public ParticipationType ParticipationType { get; set; }

        public DeadlineType DeadlineType { get; set; }

        public string Status { get; set; }

        public string Finalized { get; set; }
    }
}