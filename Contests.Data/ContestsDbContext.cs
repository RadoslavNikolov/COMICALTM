namespace Contests.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
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

        public virtual IDbSet<Photo> Photos { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Contest> Contests { get; set; }

        public virtual IDbSet<Vote> Votes { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Contest>()
                .HasMany(u => u.Participants)
                .WithMany(c => c.ContestsParticipated)
                .Map(uf =>
                {
                    uf.MapLeftKey("ContestId");
                    uf.MapRightKey("UserId");
                    uf.ToTable("ContestsParticipants");
                });

            modelBuilder.Entity<Contest>()
                .HasMany(u => u.Voters)
                .WithMany()
                .Map(uf =>
                {
                    uf.MapLeftKey("ContestId");
                    uf.MapRightKey("UserId");
                    uf.ToTable("ContestsVoters");
                });

            modelBuilder.Entity<Contest>()
                .HasMany(u => u.Winners)
                .WithMany()
                .Map(uf =>
                {
                    uf.MapLeftKey("ContestId");
                    uf.MapRightKey("UserId");
                    uf.ToTable("ContestsWinners");
                });

            //modelBuilder.Entity<Vote>()
            //    .HasRequired(v => v.Photo)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);
                
                

            base.OnModelCreating(modelBuilder);
        }
    }
}