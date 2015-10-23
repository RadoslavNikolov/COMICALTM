namespace Contests.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }
    }
}