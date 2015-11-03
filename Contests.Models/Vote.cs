namespace Contests.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Vote
    {
        public int Id { get; set; }

        public int PhotoId { get; set; }

        public virtual Photo Photo { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
