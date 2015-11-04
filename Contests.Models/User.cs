namespace Contests.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Photo> photos;
        private ICollection<Contest> contestsCreated;
        private ICollection<Contest> contestsParticipated;
        private ICollection<Notification> notifications; 

        public User()
        {
            this.photos = new HashSet<Photo>();
            this.contestsCreated = new HashSet<Contest>();
            this.contestsParticipated = new HashSet<Contest>();
            this.RegisteredOn = DateTime.Now;
            this.notifications = new HashSet<Notification>();
        }

        [Required]
        public string FullName { get; set; }

        public string ProfilePhotoPath { get; set; }

        public string ThumbnailPath { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        //public int? ProfileImageId { get; set; }

        //public virtual Photo ProfileImage { get; set; }

        public virtual ICollection<Photo> Photos
        {
            get { return this.photos; }
            set { this.photos = value; }
        }

        public virtual ICollection<Contest> ContestsCreated
        {
            get { return this.contestsCreated; }
            set { this.contestsCreated = value; }
        }

        public virtual ICollection<Contest> ContestsParticipated
        {
            get { return this.contestsParticipated; }
            set { this.contestsParticipated = value; }
        }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}