namespace Contests.Data.Interfaces
{
    using System;

    public interface IContestsDbContext : IDisposable
    {
        int SaveChanges();
    }
}