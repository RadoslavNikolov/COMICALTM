namespace Contests.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App.Controllers;
    using App.Infrastructure.UserIdProvider;
    using App.Models.BindingModels;
    using Data.UnitOfWork;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Models.Enums;
    using Moq;

    [TestClass]
    public class ContestControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();
        }

        [TestMethod]
        public void CreateContest_WithValidData_ShouldSuccessfullyAddTheContest()
        {
            IList<Contest> contests = new List<Contest>();
            var organizator = this.mocks
                .UserRepositoryMock
                .Object
                .All()
                .FirstOrDefault();
            if (organizator == null)
            {
                Assert.Fail("Cannot perform test - no users available");
            }

            this.mocks.ContestRepositoryMock.Setup(c => c.Add(It.IsAny<Contest>()))
                .Callback((Contest contest) =>
                {
                    contest.Organizator = organizator;
                    contests.Add(contest);
                });

            var mockContext = new Mock<IContestsData>();
            mockContext.Setup(c => c.Contests)
                .Returns(this.mocks.ContestRepositoryMock.Object);
            mockContext.Setup(c => c.Categories)
                .Returns(this.mocks.CategoryRepositoryMock.Object);
            mockContext.Setup(c => c.Users)
                .Returns(this.mocks.UserRepositoryMock.Object);

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId())
                .Returns(organizator.Id);

            var contestController = new ContestController(mockContext.Object, mockIdProvider.Object);

            string contestTitle = Guid.NewGuid().ToString();
            var category = mockContext.Object
                .Categories
                .All()
                .FirstOrDefault();
            if (category == null)
            {
                Assert.Fail("Cannot perform test - no categories available");
            }

            var newContest = new ContestBindingModel
            {
                Title = contestTitle,
                Description = "Test contest",
                RewardType = RewardType.SingleWinner,
                VotingType = VotingType.Open,
                ParticipationType = ParticipationType.Open,
                DeadlineType = DeadlineType.ByParticipants,
                ParticipantsNumberDeadline = 5,
                Category = category.Id
            };

            ActionResult response = contestController.Create(newContest);

            var contestFromRepo = contests.FirstOrDefault(c => c.Title == newContest.Title);
            if (contestFromRepo == null)
            {
                Assert.Fail();
            }

            Assert.AreEqual(1, contests.Count);
            Assert.IsInstanceOfType(response, typeof(RedirectToRouteResult));
            Assert.AreEqual(newContest.Description, contestFromRepo.Description);
        }

        [TestMethod]
        public void CreateContest_WithInvalidData_ShouldReturnViewWithoutCreatingNewContest()
        {
            IList<Contest> contests = new List<Contest>();
            var organizator = this.mocks
                .UserRepositoryMock
                .Object
                .All()
                .FirstOrDefault();
            if (organizator == null)
            {
                Assert.Fail("Cannot perform test - no users available");
            }

            this.mocks.ContestRepositoryMock.Setup(c => c.Add(It.IsAny<Contest>()))
                .Callback((Contest contest) =>
                {
                    contests.Add(contest);
                });

            var mockContext = new Mock<IContestsData>();
            mockContext.Setup(c => c.Contests)
                .Returns(this.mocks.ContestRepositoryMock.Object);
            mockContext.Setup(c => c.Categories)
                .Returns(this.mocks.CategoryRepositoryMock.Object);
            mockContext.Setup(c => c.Users)
                .Returns(this.mocks.UserRepositoryMock.Object);

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId())
                .Returns(organizator.Id);

            var contestController = new ContestController(mockContext.Object, mockIdProvider.Object);

            var newContest = new ContestBindingModel
            {
                Description = "Test contest",
                Category = -1
            };

            contestController.Create(newContest);

            mockContext.Verify(m => m.SaveChanges(), Times.Never);

            Assert.AreEqual(0, contests.Count);
        }
    }
}
