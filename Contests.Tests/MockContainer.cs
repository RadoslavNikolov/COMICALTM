namespace Contests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Interfaces;
    using Models;
    using Models.Enums;
    using Moq;

    public class MockContainer
    {
        public Mock<IRepository<Category>> CategoryRepositoryMock { get; set; }

        public Mock<IRepository<Contest>> ContestRepositoryMock { get; set; }

        public Mock<IRepository<User>> UserRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetUpFakeCategories();
            this.SetUpFakeContests();
            this.SetUpFakeUsers();
        }

        private void SetUpFakeUsers()
        {
            var fakeUsers = this.AddFakeUsers();

            this.UserRepositoryMock = new Mock<IRepository<User>>();
            this.UserRepositoryMock.Setup(u => u.All())
                .Returns(fakeUsers.AsQueryable());
            this.UserRepositoryMock.Setup(u => u.Find(It.IsAny<string>()))
                .Returns((string id) =>
                {
                    var user = fakeUsers.FirstOrDefault(u => u.Id == id);

                    return user;
                });
        }

        private void SetUpFakeContests()
        {
            var fakeContests = this.AddFakeContests();

            this.ContestRepositoryMock = new Mock<IRepository<Contest>>();
            this.ContestRepositoryMock.Setup(c => c.All())
                .Returns(fakeContests.AsQueryable());
            this.ContestRepositoryMock.Setup(c => c.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var contest = fakeContests.FirstOrDefault(c => c.Id == id);

                    return contest;
                });
        }

        private void SetUpFakeCategories()
        {
            var fakeCategories = this.AddFakeCategories();

            this.CategoryRepositoryMock = new Mock<IRepository<Category>>();
            this.CategoryRepositoryMock.Setup(c => c.All())
                .Returns(fakeCategories.AsQueryable());
            this.CategoryRepositoryMock.Setup(c => c.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var category = fakeCategories.FirstOrDefault(c => c.Id == id);

                    return category;
                });
        }

        private List<Category> AddFakeCategories()
        {
            var fakeContests = this.AddFakeContests();
            var fakeCategories = new List<Category>
            {
                new Category { Name = "Nature" , IsActive = true, Contests = AddFakeContests()},
                new Category { Name = "Music" , IsActive = false}
            };

            return fakeCategories;
        }

        private List<Contest> AddFakeContests()
        {
            var users = this.AddFakeUsers();

            var contests = new List<Contest>
            {
                new Contest
                {
                    Title = "Fake contest",
                    Description = "This is the best fake contest.",
                    IsActive = true,
                    CategoryId = 1,
                    CreatedOn = DateTime.Now,
                    RewardType = RewardType.TopNWinners,
                    ParticipationType = ParticipationType.Closed,
                    VotingType = VotingType.Closed,
                    DeadlineType = DeadlineType.ByParticipants,
                    WinnersCount = 3,
                    ParticipantsNumberDeadline = 5,
                    Participants = users,
                    Voters = users,
                    OrganizatorId = users.First().Id
                },
                 new Contest
                {
                    Title = "Another fake contest",
                    Description = "Nothing much to say.",
                    IsActive = true,
                    CategoryId = 2,
                    CreatedOn = DateTime.Now,
                    RewardType = RewardType.SingleWinner,
                    ParticipationType = ParticipationType.Open,
                    VotingType = VotingType.Open,
                    DeadlineType = DeadlineType.ByTime,
                    DeadLine = DateTime.Now.AddDays(3),
                    OrganizatorId = users.First().Id
                }
            };

            return contests;
        }

        private List<User> AddFakeUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    UserName = "nikola",
                    Id = "1"
                },
                new User()
                {
                    UserName = "radoslav",
                    Id = "2"
                },
                new User()
                {
                    UserName = "martin",
                    Id = "3"
                }
            };

            return users;
        }
    }
}
