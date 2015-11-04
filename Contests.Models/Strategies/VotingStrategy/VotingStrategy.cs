namespace Contests.Models.Strategies.VotingStrategy
{
    using System.Linq;

    public abstract class VotingStrategy
    {
        protected VotingStrategy(Contest contest, Photo photo, string userId)
        {
            this.Contest = contest;
            this.Photo = photo;
            this.UserId = userId;
        }

        public Contest Contest { get; set; }

        public Photo Photo { get; set; }

        public string UserId { get; set; }

        public virtual bool CanVote()
        {
            bool hasAlreadyVoted = this.Contest.Votes.Any(v => v.UserId == this.UserId);
            bool isPhotoOwner = this.Photo.OwnerId == this.UserId;
            bool isPhotoDeleted = this.Photo.IsDeleted;

            if (hasAlreadyVoted || isPhotoOwner || isPhotoDeleted)
            {
                return false;
            }

            return true;
        }
    }
}
