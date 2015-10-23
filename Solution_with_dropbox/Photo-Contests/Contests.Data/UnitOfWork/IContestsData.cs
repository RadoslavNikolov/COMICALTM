namespace Contests.Data.UnitOfWork
{
    using Interfaces;
    using Models;

    public interface IContestsData
    {
        IRepository<User> Users { get; }

        IRepository<File> Files { get; }

        IRepository<Photo> Photos { get; } 

        void SaveChanges();
    }
}