namespace Contests.Models
{
    using System.ComponentModel.DataAnnotations;

    public class File
    {
        [Key]
        public int FileId { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

    }
}