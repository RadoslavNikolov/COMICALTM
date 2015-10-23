﻿namespace Contests.Data
{
    using System.Data.Entity;
    using Interfaces;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class ContestsDbContext : IdentityDbContext<User>, IContestsDbContext
    {

        public ContestsDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ContestsDbContext Create()
        {
            return new ContestsDbContext();
        }


        public virtual IDbSet<File> Files { get; set; }
        public virtual IDbSet<Photo> Photos { get; set; }
        
    }
}