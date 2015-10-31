﻿namespace Contests.Models
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

        public User()
        {
            this.photos = new HashSet<Photo>();
            this.RegisteredOn = DateTime.Now;
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}