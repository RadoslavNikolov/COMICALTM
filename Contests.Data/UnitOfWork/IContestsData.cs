namespace Contests.Data.UnitOfWork
{
    using Interfaces;
    using Models;

    public interface IContestsData
    {
        IRepository<User> Users { get; }

        IRepository<File> Files { get; }

        void SaveChanges();
    }
}