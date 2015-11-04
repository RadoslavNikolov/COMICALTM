namespace Contests.Data.UnitOfWork
{
    using Interfaces;
    using Models;

    public interface IContestsData
    {
        IRepository<User> Users { get; }

        IRepository<Photo> Photos { get; }

        IRepository<Category> Categories { get; } 

        IRepository<Contest> Contests { get; }

        IRepository<Vote> Votes { get; } 

        IRepository<Notification> Notifications { get; }

        void SaveChanges();
    }
}